using Microsoft.FlightSimulator.SimConnect;
using SimConnectHelper.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SimConnectHelper
{
    /// <summary>
    /// Static class to connect/disconnect to MSFS 2020, via SimConnect
    /// </summary>
    public static class SimConnectHandler
    {
        private static MessageHandler handler = null;
        private static CancellationTokenSource source = null;
        private static CancellationToken token = CancellationToken.None;
        private static Task messagePump;
        private static EndPoint endPoint;
        private static AutoResetEvent messagePumpRunning = new AutoResetEvent(false);
        private static SimConnect simConnect = null;
        private const int WM_USER_SIMCONNECT = 0x0402;
        private static int RequestID = 0;
        private static List<System.Threading.Timer> timers = new List<System.Threading.Timer>();
        private static List<SimVarTimer> timerStates = new List<SimVarTimer>();
        public static Dictionary<int, SimConnectVariable> Requests { get; private set; } = new Dictionary<int, SimConnectVariable>();
        /// <summary>
        /// Allow connections to older MSFS applications (untested)
        /// </summary>
        public static bool UseFSXcompatibleConnection { get; set; } = false;
        /// <summary>
        /// Indicate if we have established a connection to MSFS 2020
        /// </summary>
        public static bool IsConnected { get; private set; } = false;
        /// <summary>
        /// Details of how we connected to MSFS 2020 (or the last connection type attempted)
        /// </summary>
        public static SimConnectConfig Connection { get; private set; }
        /// <summary>
        /// How often should SimConnect update the values for requested SimVars
        /// </summary>
        public static SimConnectUpdateFrequency DefaultUpdateFrequency { get; set; } = SimConnectUpdateFrequency.SIM_Frame;
        /// <summary>
        /// Called whenever SimConnect connects or disconnects with MSFS 2020
        /// </summary>
        public static EventHandler<bool> SimConnected;
        /// <summary>
        /// Called whenever SimConnect receives an error from MSFS 2020
        /// </summary>
        public static EventHandler<IOException> SimError;
        /// <summary>
        /// Called whenever MSFS 2020 transmits requested data about an object (e.g. SimVar result)
        /// </summary>
        public static EventHandler<SimConnectVariableValue> SimData;
        /// <summary>
        /// Provides feedback on progress and actions taken by/with SimConnect.
        /// Useful for debugging and seeing how SimConnect communication works.
        /// </summary>
        public static EventHandler<SimVarMessage> SimLog;
        /// <summary>
        /// Full path and filename to use for saving a Config file
        /// </summary>
        public static string ConfigFilePath { get; set; } = Path.Combine(Environment.CurrentDirectory, "SimConnect.cfg");

        /// <summary>
        /// Attempts to connect to a local instance of MSFS 2020, using each XML-defined connection, until one connects
        /// </summary>
        public static void Connect()
        {
            WriteLog("Start Connect()");
            foreach(var config in GetLocalFSConnections())
            {
                Connect(config);
                var timeout = DateTime.Now.AddSeconds(1);
                while (!IsConnected && DateTime.Now < timeout)
                    Thread.Sleep(50);
                if (IsConnected)
                    break;
            }
            WriteLog("End Connect()");
        }

        /// <summary>
        /// Creates a SimConnect.cfg file and attempts to connect SimConnect to MSFS 2020.
        /// Only creates a SimConnect.cfg if Server IP or Port are different from last connection
        /// </summary>
        /// <param name="ep">MSFS 2020 SimConnect Server IP & Port, NULL forces the re-use of a previously saved Config</param>
        public static void Connect(EndPoint ep)
        {
            WriteLog("Start Connect(EndPoint)");
            if (ep != null)
                endPoint = ep;
            Connect((SimConnectConfig)null);
            WriteLog("End Connect(EndPoint)");
        }

        public static void Connect(SimConnectConfig config)
        {
            WriteLog("Start Connect(SimConnectConfig)");
            if (source != null)
                Disconnect();
            Connection = config;
            CreateConfigFile(config);
            source = new CancellationTokenSource();
            token = source.Token;
            token.ThrowIfCancellationRequested();
            messagePump = new Task(RunMessagePump, token);
            messagePump.Start();
            messagePumpRunning = new AutoResetEvent(false);
            messagePumpRunning.WaitOne();
            WriteLog("End Connect(SimConnectConfig)");
        }

        /// <summary>
        /// Will stop the Message Handler from running, thereby ignoring all events from SimConnect
        /// </summary>
        public static void Disconnect()
        {
            WriteLog("Start Disconnect()");
            StopMessagePump();
            // Raise event to notify client we've disconnected
            SimConnect_OnRecvQuit(simConnect, null);
            simConnect?.Dispose(); // May have already been disposed or not even been created, e.g. Disconnect called before Connect
            simConnect = null;
            WriteLog("End Disconnect()");
        }

        private static void StopMessagePump()
        {
            WriteLog("Start StopMessagePump()");
            if (source != null && token.CanBeCanceled)
            {
                source.Cancel();
            }
            if (messagePump != null)
            {
                handler.Stop();
                handler = null;

                messagePumpRunning.Close();
                messagePumpRunning.Dispose();
            }
            messagePump = null;
            WriteLog("End StopMessagePump()");
        }

        /// <summary>
        /// Will attempt to delete an existing SimConnect.cfg file, if not in use
        /// </summary>
        /// 
        private static bool DeleteConfigFile()
        {
            WriteLog("Start DeleteConfigFile()");
            var filePath = GetConfigFilePath();// Path.Combine(Environment.CurrentDirectory, "SimConnect.cfg");
            if (File.Exists(filePath))
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("File Delete Error: {0}\r\nEnd DeleteConfigFile()", ex.Message), EventLogEntryType.Error);
                    return false;
                }
            WriteLog("End DeleteConfigFile()");
            return true;
        }

        private static void CreateConfigFile(SimConnectConfig config = null)
        {
            WriteLog("Start CreateConfigFile(SimConnectConfig)");
            if (config == null)
                config = new SimConnectConfig
                {
                    Descr = "Dynamic Config",
                    Address = ((IPEndPoint)endPoint).Address.ToString(),
                    Port = ((IPEndPoint)endPoint).Port.ToString()
                };
            var filePath = GetConfigFilePath();
            File.WriteAllText(filePath, config.ConfigFileText);
            WriteLog("End CreateConfigFile(SimConnectConfig)");
        }

        /// <summary>
        /// If not already connected, will attempt to connect ONCE to MSFS2020
        /// </summary>
        private static void RunMessagePump()
        {
            WriteLog("Start RunMessagePump()");
            // Create control to handle windows messages
            if (!IsConnected)
            {
                handler = new MessageHandler();
                handler.CreateHandle();
                ConnectFS(handler);
            }
            messagePumpRunning.Set();
            WriteLog("End RunMessagePump()");
            Application.Run();
        }

        /// <summary>
        /// Create an instance of SimConnect, if successful, attaches all event handlers
        /// </summary>
        /// <param name="messageHandler">Windows Message Handler</param>
        private static void ConnectFS(MessageHandler messageHandler)
        {
            WriteLog("Start ConnectFS(MessageHandler)");
            // SimConnect must be linked in the same thread as the Application.Run()
            try
            {
                simConnect = new SimConnect("RemoteClient", messageHandler.Handle, WM_USER_SIMCONNECT, null, UseFSXcompatibleConnection ? (uint)1 : 0);

                messageHandler.MessageReceived += MessageReceived;

                /// Listen for Connect
                simConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);

                /// Listen for Disconnect
                simConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                /// Listen for Exceptions
                simConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                /// Listen for SimVar Data
                simConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataByType);

                /// Listen for SimVar Data
                simConnect.OnRecvSimobjectData += new SimConnect.RecvSimobjectDataEventHandler(SimConnect_OnRecvSimobjectData);
            }
            catch (Exception ex)
            {
                // Is MSFS is not running, a COM Exception is raised. We ignore it!
                WriteLog(string.Format("Connect Error: {0}", ex.Message), EventLogEntryType.Error);
            }
            WriteLog("End ConnectFS(MessageHandler)");
        }

        private static void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            //WriteLog("Start SimConnect_OnRecvSimobjectData(SimConnect, SIMCONNECT_RECV_SIMOBJECT_DATA)");
            if (simConnect != null) {
                var newData = new SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE
                {
                    dwData = data.dwData,
                    dwDefineCount = data.dwDefineCount,
                    dwDefineID = data.dwDefineID,
                    dwentrynumber = data.dwentrynumber,
                    dwFlags = data.dwFlags,
                    dwID = data.dwID,
                    dwObjectID = data.dwObjectID,
                    dwoutof = data.dwoutof,
                    dwRequestID = data.dwRequestID,
                    dwSize = data.dwSize,
                    dwVersion = data.dwVersion
                };
                SimConnect_OnRecvSimobjectDataByType(sender, newData);
            }
            //WriteLog("End SimConnect_OnRecvSimobjectData(SimConnect, SIMCONNECT_RECV_SIMOBJECT_DATA)");
        }

        /// <summary>
        /// When SimConnect sends an updated object, the data is captured here
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="data">Object Data</param>
        private static void SimConnect_OnRecvSimobjectDataByType(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            //WriteLog("Start SimConnect_OnRecvSimobjectDataByType(SimConnect, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE)");
            if (SimData != null)
                try
                {
                    var value = data?.dwData;
                    if (value != null && value.Length == 1 && value[0].GetType() == typeof(SimVarString))
                        value = new object[] { ((SimVarString)value[0]).Value };
                    var simVarVal = new SimConnectVariableValue
                    {
                        Request = Requests[(int)data.dwRequestID],
                        Value = value
                    };
                    new Task(new Action(() => SimData.DynamicInvoke(simConnect, simVarVal))).Start();
                }
                catch(Exception ex)
                {
                    WriteLog(string.Format("SimConnect_OnRecvSimobjectDataByType Error: {0}", ex.Message), EventLogEntryType.Error);
                }
            //WriteLog("End SimConnect_OnRecvSimobjectDataByType(SimConnect, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE)");
        }

        /// <summary>
        /// When SimConnect encounters an error, it is captured here.
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="data">Error details</param>
        private static void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            WriteLog("Start SimConnect_OnRecvException(SimConnect, SIMCONNECT_RECV_EXCEPTION)");
            if (SimError != null)
                try
                {
                    var ex = new IOException("SimConnect returned an Error, details in Data", null);
                    ex.Source = "SimConnect";
                    ex.Data.Add("data", data);
                    foreach (var property in data.GetType().GetFields())
                    {
                        ex.Data.Add(property.Name, property.GetValue(data));
                    }
                    var exceptionType = string.Empty;
                    switch (Convert.ToInt32(data.dwException))
                    {
                        case 0:
                            exceptionType = "SIMCONNECT_EXCEPTION_NONE";
                            break;
                        case 1:
                            exceptionType = "SIMCONNECT_EXCEPTION_ERROR";
                            break;
                        case 2:
                            exceptionType = "SIMCONNECT_EXCEPTION_SIZE_MISMATCH";
                            break;
                        case 3:
                            exceptionType = "SIMCONNECT_EXCEPTION_UNRECOGNIZED_ID";
                            break;
                        case 4:
                            exceptionType = "SIMCONNECT_EXCEPTION_UNOPENED";
                            break;
                        case 5:
                            exceptionType = "SIMCONNECT_EXCEPTION_VERSION_MISMATCH";
                            break;
                        case 6:
                            exceptionType = "SIMCONNECT_EXCEPTION_TOO_MANY_GROUPS";
                            break;
                        case 7:
                            exceptionType = "SIMCONNECT_EXCEPTION_NAME_UNRECOGNIZED";
                            break;
                        case 8:
                            exceptionType = "SIMCONNECT_EXCEPTION_TOO_MANY_EVENT_NAMES";
                            break;
                        case 9:
                            exceptionType = "SIMCONNECT_EXCEPTION_EVENT_ID_DUPLICATE";
                            break;
                        case 10:
                            exceptionType = "SIMCONNECT_EXCEPTION_TOO_MANY_MAPS";
                            break;
                        case 11:
                            exceptionType = "SIMCONNECT_EXCEPTION_TOO_MANY_OBJECTS";
                            break;
                        case 12:
                            exceptionType = "SIMCONNECT_EXCEPTION_TOO_MANY_REQUESTS";
                            break;
                        case 13:
                            exceptionType = "SIMCONNECT_EXCEPTION_WEATHER_INVALID_PORT";
                            break;
                        case 14:
                            exceptionType = "SIMCONNECT_EXCEPTION_WEATHER_INVALID_METAR";
                            break;
                        case 15:
                            exceptionType = "SIMCONNECT_EXCEPTION_WEATHER_UNABLE_TO_GET_OBSERVATION";
                            break;
                        case 16:
                            exceptionType = "SIMCONNECT_EXCEPTION_WEATHER_UNABLE_TO_CREATE_STATION";
                            break;
                        case 17:
                            exceptionType = "SIMCONNECT_EXCEPTION_WEATHER_UNABLE_TO_REMOVE_STATION";
                            break;
                        case 18:
                            exceptionType = "SIMCONNECT_EXCEPTION_INVALID_DATA_TYPE";
                            break;
                        case 19:
                            exceptionType = "SIMCONNECT_EXCEPTION_INVALID_DATA_SIZE";
                            break;
                        case 20:
                            exceptionType = "SIMCONNECT_EXCEPTION_DATA_ERROR";
                            break;
                        case 21:
                            exceptionType = "SIMCONNECT_EXCEPTION_INVALID_ARRAY";
                            break;
                        case 22:
                            exceptionType = "SIMCONNECT_EXCEPTION_CREATE_OBJECT_FAILED";
                            break;
                        case 23:
                            exceptionType = "SIMCONNECT_EXCEPTION_LOAD_FLIGHTPLAN_FAILED";
                            break;
                        case 24:
                            exceptionType = "SIMCONNECT_EXCEPTION_OPERATION_INVALID_FOR_OJBECT_TYPE";
                            break;
                        case 25:
                            exceptionType = "SIMCONNECT_EXCEPTION_ILLEGAL_OPERATION";
                            break;
                        case 26:
                            exceptionType = "SIMCONNECT_EXCEPTION_ALREADY_SUBSCRIBED";
                            break;
                        case 27:
                            exceptionType = "SIMCONNECT_EXCEPTION_INVALID_ENUM";
                            break;
                        case 28:
                            exceptionType = "SIMCONNECT_EXCEPTION_DEFINITION_ERROR";
                            break;
                        case 29:
                            exceptionType = "SIMCONNECT_EXCEPTION_DUPLICATE_ID";
                            break;
                        case 30:
                            exceptionType = "SIMCONNECT_EXCEPTION_DATUM_ID";
                            break;
                        case 31:
                            exceptionType = "SIMCONNECT_EXCEPTION_OUT_OF_BOUNDS";
                            break;
                        case 32:
                            exceptionType = "SIMCONNECT_EXCEPTION_ALREADY_CREATED";
                            break;
                        case 33:
                            exceptionType = "SIMCONNECT_EXCEPTION_OBJECT_OUTSIDE_REALITY_BUBBLE";
                            break;
                        case 34:
                            exceptionType = "SIMCONNECT_EXCEPTION_OBJECT_CONTAINER";
                            break;
                        case 35:
                            exceptionType = "SIMCONNECT_EXCEPTION_OBJECT_AI";
                            break;
                        case 36:
                            exceptionType = "SIMCONNECT_EXCEPTION_OBJECT_ATC";
                            break;
                        case 37:
                            exceptionType = "SIMCONNECT_EXCEPTION_OBJECT_SCHEDULE";
                            break;
                    }
                    ex.Data.Add("exceptionType", exceptionType);
                    SimError.DynamicInvoke(simConnect, ex);
                }
                catch(Exception ex)
                {
                    WriteLog(string.Format("Message Receive Error: {0}", ex.Message), EventLogEntryType.Error);
                }
            WriteLog("End SimConnect_OnRecvException(SimConnect, SIMCONNECT_RECV_EXCEPTION)");
        }

        /// <summary>
        /// When SimConnect successfully connects to MSFS 2020, this event is triggered
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="data">Connection info</param>
        private static void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            WriteLog("Start SimConnect_OnRecvOpen(SimConnect, SIMCONNECT_RECV_OPEN)");
            IsConnected = true;
            if (SimConnected != null)
                try
                {
                    SimConnected.DynamicInvoke(simConnect, true);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("Message Receive Error: {0}", ex.Message), EventLogEntryType.Error);
                }
            WriteLog("End SimConnect_OnRecvOpen(SimConnect, SIMCONNECT_RECV_OPEN)");
        }

        /// <summary>
        /// When SimConnect loses connection to MSFS 2020, this event is triggered.
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="data">connection data</param>
        private static void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            WriteLog("Start SimConnect_OnRecvQuit(SimConnect, SIMCONNECT_RECV)");
            IsConnected = false;
            if (SimConnected != null)
                try
                {
                    SimConnected.DynamicInvoke(simConnect, false);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("Message Receive Error: {0}", ex.Message), EventLogEntryType.Error);
                }
            WriteLog("End SimConnect_OnRecvQuit(SimConnect, SIMCONNECT_RECV)");
        }

        /// <summary>
        /// Request a SimVariable from SimConnect, optionally start capturing values
        /// </summary>
        /// <param name="request">SimVar to fetch from SimConnect</param>
        /// <param name="frequency">How frequently should SimConnect provide an updated value?</param>
        /// <returns>A unique ID for the submitted request. Use this to request the next value via FetchValueUpdate</returns>
        public static int GetSimVar(SimConnectVariable request, SimConnectUpdateFrequency frequency = SimConnectUpdateFrequency.Never)
        {
            WriteLog("Start GetSimVar(SimConnectVariable, SimConnectUpdateFrequency)");
            if (IsConnected)
            {
                var unit = request.Unit;
                if (unit?.IndexOf("string") > -1)
                {
                    unit = null;
                }
                SimVarRequest simReq;
                lock (Requests)
                {
                    if (Requests.Any(x => x.Value.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase)
                        && x.Value.Unit.Equals(request.Unit, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        // Re-use a previously requested variable for retransmission to SimConnect
                        var reqId = GetRequestId(request);
                        simReq = new SimVarRequest
                        {
                            ID = reqId,
                            Request = request
                        };
                    }
                    else
                    {
                        // Fetch the values suitable for transmission to SimConnect
                        simReq = new SimVarRequest
                        {
                            ID = RequestID++,
                            Request = request
                        };
                        // New SimVar requested - add it to our list
                        Requests.Add((int)simReq.ReqID, simReq.Request);
                    }
                }
                // Submit the SimVar request to SimConnect
                // m_oSimConnect.AddToDataDefinition(_oSimvarRequest.eDef, _oSimvarRequest.sName, _oSimvarRequest.sUnits, SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                try
                {
                    simConnect.AddToDataDefinition(simReq.DefID, request.Name, unit, simReq.SimType, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    // Tell SimConnect what type of value we are expecting to be returned
                    switch (simReq.DataType?.FullName)
                    {
                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                            simConnect.RegisterDataDefineStruct<uint>(simReq.DefID);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                            simConnect.RegisterDataDefineStruct<int>(simReq.DefID);
                            break;
                        case "System.Boolean":
                            simConnect.RegisterDataDefineStruct<bool>(simReq.DefID);
                            break;
                        case "System.Byte":
                            simConnect.RegisterDataDefineStruct<byte>(simReq.DefID);
                            break;
                        case "System.String":
                            simConnect.RegisterDataDefineStruct<SimVarString>(simReq.DefID);
                            break;
                        case "System.Object":
                            simConnect.RegisterDataDefineStruct<object>(simReq.DefID); // This will likely fail as variants don't transform well
                            break;
                        default:
                            simConnect.RegisterDataDefineStruct<double>(simReq.DefID); // We'll presume default values being requested are numeric
                            break;
                    }
                    if (frequency != SimConnectUpdateFrequency.Never)
                        GetSimVar(simReq.ID, DefaultUpdateFrequency); // Request value to be sent back immediately, will auto-update using pre-defined frequency
                }
                catch(Exception ex)
                {
                    WriteLog(string.Format("SimConnect Error: {0}\r\nEnd GetSimVar(SimConnectVariable, SimConnectUpdateFrequency)", ex.Message), EventLogEntryType.Error);
                    SimConnect_OnRecvException(simConnect, new SIMCONNECT_RECV_EXCEPTION { dwException = (uint)ex.HResult });
                    return -1;
                }
                WriteLog("End GetSimVar(SimConnectVariable, SimConnectUpdateFrequency)");
                return simReq.ID;
            }
            WriteLog("SimVar Not Found\r\nEnd GetSimVar(SimConnectVariable, SimConnectUpdateFrequency)", EventLogEntryType.Warning);
            return -1;
        }

        /// <summary>
        /// Request a SimVar value using a custom-defined frequency
        /// </summary>
        /// <param name="request">SimVar to request</param>
        /// <param name="frequencyInMs">Frequency (in ms)</param>
        /// <returns></returns>
        public static int GetSimVar(SimConnectVariable request, int frequencyInMs)
        {
            WriteLog("Start GetSimVar(SimConnectVariable, int)");
            if (frequencyInMs > (int)SIMCONNECT_PERIOD.VISUAL_FRAME)
            {
                return GetSimVar(request, (SimConnectUpdateFrequency)frequencyInMs);
            }
            WriteLog(string.Format("GetSimVar(SimConnectVariable, int) error: Frequency must be > {0} ms", (int)SIMCONNECT_PERIOD.VISUAL_FRAME), EventLogEntryType.Warning);
            WriteLog("End GetSimVar(SimConnectVariable, int)");
            return -1;
        }

        /// <summary>
        /// Allows cancelling of a previously requested variable, if it is no-longer needed
        /// </summary>
        /// <param name="request">SimVar Request to cancel</param>
        public static bool CancelRequest(SimConnectVariable request)
        {
            WriteLog("Start CancelRequest(SimConnectVariable)");
            var result = false;
            if (simConnect != null && IsConnected && Requests.Any(x => x.Value.Name == request.Name && x.Value.Unit == request.Unit))
            {
                lock (Requests)
                {
                    try
                    {
                        var submittedRequest = Requests.First(x => x.Value.Name == request.Name && x.Value.Unit == request.Unit);
                        var requestId = submittedRequest.Key;
                        //simConnect.ClearDataDefinition((SIMVARDEFINITION)requestId);
                        simConnect.ClearClientDataDefinition((SIMVARDEFINITION)requestId);
                        Requests.Remove(requestId);
                        if( timerStates.Any(x => x.RequestID == requestId))
                        {
                            RemoveTimer(requestId);
                        }
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        WriteLog(string.Format("Cancellation Error: {0}", ex.Message), EventLogEntryType.Error);
                    }
                }
            }
            WriteLog("End CancelRequest(SimConnectVariable)");
            return result;
        }

        private static void RemoveTimer(int requestId)
        {
            WriteLog("Start RemoveTimer(int)");
            var timerId = timerStates.FirstOrDefault(x => x.RequestID == requestId)?.TimerID;
            if (timerId != null)
                lock (timers)
                {
                    timers[(int)timerId].Dispose();
                    lock (timerStates)
                        foreach (var timerState in timerStates.Where(x => x.TimerID > (int)timerId))
                        {
                            timerState.TimerID = timerState.TimerID--;
                        }
                }
            WriteLog("End RemoveTimer(int)");
        }

        /// <summary>
        /// Request an update for a specific SimVar request
        /// </summary>
        /// <param name="requestID">ID returned by SendRequest</param>
        /// <param name="frequency">SimVar can be requested manually (SimConnectUpdateFrequency.Never) or auto-sent at a pre-defined frequency</param>
        public static void GetSimVar(int requestID, SimConnectUpdateFrequency frequency)
        {
            WriteLog("Start GetSimVar(int, SimConnectUpdateFrequency)");
            try
            {
                if (IsConnected)
                    if (frequency == SimConnectUpdateFrequency.Never)
                        simConnect?.RequestDataOnSimObjectType((SIMVARREQUEST)requestID, (SIMVARDEFINITION)requestID, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
                    else
                    {
                        if ((int)frequency <= (int)SIMCONNECT_PERIOD.VISUAL_FRAME)
                        {
                            SIMCONNECT_PERIOD period = Enum.Parse<SIMCONNECT_PERIOD>(frequency.ToString().ToUpper());
                            simConnect?.RequestDataOnSimObject((SIMVARREQUEST)requestID, (SIMVARDEFINITION)requestID, 0, period, SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT, 0, 0, 0);
                        }
                        else
                        {
                            // Not a pre-defined interval - use a timer
                            var tmr = new System.Threading.Timer(GetSimVar, new SimVarTimer { RequestID = requestID, Request = Requests[requestID], FrequencyInMs = (int)frequency }, (int)frequency, (int)frequency);
                        }
                    }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("Get SimVar Error: {0}", ex.Message), EventLogEntryType.Error);
                // Likely cause, no request for this variable has previously been submitted
            }
            WriteLog("End GetSimVar(int, SimConnectUpdateFrequency)");
        }

        private static void GetSimVar(object state)
        {
            try
            {
                var requestID = ((SimVarTimer)state).RequestID;
                simConnect?.RequestDataOnSimObjectType((SIMVARREQUEST)requestID, (SIMVARDEFINITION)requestID, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            }
            catch(Exception ex)
            {
                WriteLog(string.Format("GetSimvar(object) error: {0}", ex.Message), EventLogEntryType.Error);
                RemoveTimer(((SimVarTimer)state).RequestID);
            }
        }

        /// <summary>
        /// Request an update for a specific SimVar request (used for GetSimVar(frequency = SIMCONNECT_PERIOD.NEVER))
        /// </summary>
        /// <param name="requestID">Variable definition requested via GetSimVar</param>
        public static void GetSimVar(SimConnectVariable request)
        {
            WriteLog("Start GetSimVar(SimConnectVariable)");
            var reqId = GetRequestId(request);
            if (reqId > -1)
            {
                GetSimVar(reqId);
            }
            else
            {
                GetSimVar(request, SimConnectUpdateFrequency.Never);
            }
            WriteLog("End GetSimVar(SimConnectVariable)");
        }

        /// <summary>
        /// Request an update for a specific SimVar request ID returned by GetSimVar(frequency = SIMCONNECT_PERIOD.NEVER)
        /// </summary>
        /// <param name="requestId">ID returned when submitting the original SimVar request</param>
        public static void GetSimVar(int requestId)
        {
            WriteLog("Start GetSimVar(int)");
            if (requestId > -1)
                GetSimVar(requestId, SimConnectUpdateFrequency.Never);
            WriteLog("End GetSimVar(int)");
        }

        /// <summary>
        /// Set the value associated with a SimVar
        /// </summary>
        /// <param name="simVarValue">SimVar and associated value</param>
        public static int SetSimVar(SimConnectVariableValue simVarValue)
        {
            WriteLog("Start SetSimVar(SimConnectVariableValue)");
            // As for requests, setting values is a 2-step process, reserve the data area,then modify the data it holds
            GetSimVar(simVarValue.Request);

            var reqId = GetRequestId(simVarValue.Request);
            if (reqId > -1)
            {
                // Data area reserved, now set the value
                simConnect.SetDataOnSimObject((SIMVARDEFINITION)reqId, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, simVarValue.Value);
                //simConnect.SetDataOnSimObject((SIMVARDEFINITION)reqId, (uint)reqId, (SIMCONNECT_DATA_SET_FLAG)SimConnect.SIMCONNECT_OBJECT_ID_USER, simVarValue.Value);
            }
            WriteLog("End SetSimVar(SimConnectVariableValue)");
            return reqId;
        }

        private static int GetRequestId(SimConnectVariable request)
        {
            WriteLog("Start GetRequestId(SimConnectVariable);\r\nEnd GetRequestId(SimConnectVariable)");
            return Requests.Any(x =>
                x.Value.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase)
                && x.Value.Unit.Equals(request.Unit, StringComparison.InvariantCultureIgnoreCase)) ?
                    Requests.FirstOrDefault(x =>
                        x.Value.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase)
                        && x.Value.Unit.Equals(request.Unit, StringComparison.InvariantCultureIgnoreCase)).Key
                : -1;
        }

        /// <summary>
        /// Every Windowws Message is captured here, we check for SimConnect messages and process them, else we ignore it
        /// </summary>
        /// <param name="sender">Windows Object generating the message</param>
        /// <param name="msg">Message from sender</param>
        private static void MessageReceived(object sender, Message msg)
        {
            //WriteLog("Start MessageReceived(object, Message)");
            if (msg.Msg == WM_USER_SIMCONNECT && simConnect != null)
                try
                {
                    // SimConnect has something to tell us - ask it to raise the relevant event
                    if (simConnect != null)
                        simConnect.ReceiveMessage();
                }
                catch// (Exception ex)
                {
                    // Seems to happen if FS is shutting down or when we disconnect
                }
            //WriteLog("End MessageReceived(object, Message)");
        }

        /// <summary>
        /// Full path & filename to the SimConnect.cfg file
        /// </summary>
        /// <returns>SimConnect.cfg FilePath</returns>
        private static string GetConfigFilePath()
        {
            // Need to confirm the correct locaion for SimConnct.cfg.
            // Some documentation states it is in the AppData folder, others within the current folder, others still state the My Documents folder
            //var filePath = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "Microsoft Flight Simulator", "SimConnect.cfg");
            WriteLog("Start GetConfigFilePath();\r\nEnd GetConfigFilePath()");
            return ConfigFilePath;
        }

        /// <summary>
        /// Read the MSFS 2020 XML Config file to find all permitted connection types
        /// </summary>
        /// <returns>List of connection configurations</returns>
        private static List<SimConnectConfig> GetLocalFSConnections()
        {
            WriteLog("Start GetLocalFSConnections()");
            List<SimConnectConfig> configs = new List<SimConnectConfig>();
            try
            {
                var filePath = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "Microsoft Flight Simulator", "SimConnect.xml");
                var fileContent = File.ReadAllText(filePath); // Load the file content instead of loading as XML - overcomes limitation with encoding
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(fileContent);
                XmlNodeList xmlNodeList = xml.SelectNodes("/SimBase.Document/SimConnect.Comm");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    try
                    {
                        SimConnectConfig config = GetConfigFromXml(xmlNode);
                        configs.Add(config);
                    }
                    catch (Exception ex)
                    {
                        WriteLog(string.Format("Unable to parse XML Connection: {0}", ex.Message), EventLogEntryType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("Unable to open XML config: {0}", ex.Message), EventLogEntryType.Error);
            }
            WriteLog("End GetLocalFSConnections()");
            return configs;
        }

        private static SimConnectConfig GetConfigFromXml(XmlNode xmlNode)
        {
            WriteLog("Start GetConfigFromXml(XmlNode)");
            XmlSerializer serial = new XmlSerializer(typeof(SimConnectConfig));
            using (XmlNodeReader reader = new XmlNodeReader(xmlNode))
            {
                WriteLog("End GetConfigFromXml(XmlNode)");
                return (SimConnectConfig)serial.Deserialize(reader);
            }
        }

        private static void WriteLog(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            if(!string.IsNullOrWhiteSpace(message) && SimLog != null)
                try
                {
                    // Send message back to client in a seperate task so it doesn't impact on performance
                    new Task(new Action(() => SimLog.DynamicInvoke(new object[] { simConnect, new SimVarMessage { Severity = type, Message = message } }))).Start();
                }
                catch { }
        }
    }
}
