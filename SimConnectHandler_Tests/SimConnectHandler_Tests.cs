using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimConnectHelper;
using SimConnectHelper.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace SimConnectHelper_Tests
{
    /// <summary>
    /// Tests will only succeed if MSFS 2020 is currently running
    /// </summary>
    [TestClass]
    public class SimConnectHandler_Tests
    {
        private const string MSFSServerName = "localhost";
        private const int MSFSServerPort = 500;

        private SimConnectVariableValue result = null;

        [TestMethod]
        public void ConnectUseLocalServerConfig_Test()
        {
            SimConnectHelper.SimConnectHelper.Connect(); // Find/Try all defined server connection configurations
            Assert.IsTrue(SimConnectHelper.SimConnectHelper.IsConnected);
        }

        [TestMethod]
        public void Disconnect_Test()
        {
            SimConnectHelper.SimConnectHelper.Connect(); // Find/Try all defined server connection configurations
            Assert.IsTrue(SimConnectHelper.SimConnectHelper.IsConnected);
            SimConnectHelper.SimConnectHelper.Disconnect();
            Assert.IsFalse(SimConnectHelper.SimConnectHelper.IsConnected);
        }

        [TestMethod]
        public void ConnectConfiguration_Test()
        {
            SimConnectHelper.SimConnectHelper.Connect(); // Find/Try all defined server connection configurations
            Assert.AreNotEqual(SimConnectHelper.SimConnectHelper.Connection.Port, "0");
        }

        [TestMethod]
        public void ConnectViaIP_Test()
        {
            SimConnectHelper.SimConnectHelper.Connect(GetEndPoint());
            Thread.Sleep(1000);
            Assert.IsTrue(SimConnectHelper.SimConnectHelper.IsConnected);
        }

        [TestMethod]
        public void RequestSimVar_Test_In_Ms()
        {
            result = null;
            SimConnectHelper.SimConnectHelper.SimError += SimConnect_Error;
            SimConnectHelper.SimConnectHelper.SimConnected += SimConnect_Connection;
            SimConnectHelper.SimConnectHelper.SimData += SimConnect_DataReceived;
            SimConnectHelper.SimConnectHelper.Connect();
            var variable = new SimConnectVariable
            {
                Name = "AMBIENT WIND VELOCITY",
                Unit = "knots"
            };
            var requestID = SimConnectHelper.SimConnectHelper.GetSimVar(variable, 50);

            // Wait up to 5 seconds for MSFS to return the requested value
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (result == null && DateTime.Now < endTime)
            {
                Thread.Sleep(100);
            }
            SimConnectHelper.SimConnectHelper.CancelRequest(variable);
            SimConnectHelper.SimConnectHelper.Disconnect();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void RequestSimVar_Test_Once()
        {
            result = null;
            SimConnectHelper.SimConnectHelper.SimError += SimConnect_Error;
            SimConnectHelper.SimConnectHelper.SimConnected += SimConnect_Connection;
            SimConnectHelper.SimConnectHelper.SimData += SimConnect_DataReceived;
            SimConnectHelper.SimConnectHelper.Connect();
            var variable = new SimConnectVariable
            {
                Name = "AMBIENT WIND VELOCITY",
                Unit = "knots"
            };
            var requestID = SimConnectHelper.SimConnectHelper.RegisterSimVar(variable, SimConnectUpdateFrequency.Once);

            // Wait up to 5 seconds for MSFS to return the requested value
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (result == null && DateTime.Now < endTime)
            {
                Thread.Sleep(100);
            }
            SimConnectHelper.SimConnectHelper.CancelRequest(variable);
            SimConnectHelper.SimConnectHelper.Disconnect();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RequestSimVar_Test_SimFrame()
        {
            result = null;
            SimConnectHelper.SimConnectHelper.SimError += SimConnect_Error;
            SimConnectHelper.SimConnectHelper.SimConnected += SimConnect_Connection;
            SimConnectHelper.SimConnectHelper.SimData += SimConnect_DataReceived;
            SimConnectHelper.SimConnectHelper.Connect();
            var variable = new SimConnectVariable
            {
                Name = "AMBIENT WIND VELOCITY",
                Unit = "knots"
            };
            var requestID = SimConnectHelper.SimConnectHelper.RegisterSimVar(variable, SimConnectUpdateFrequency.SIM_Frame);

            // Wait up to 5 seconds for MSFS to return the requested value
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (result == null && DateTime.Now < endTime)
            {
                Thread.Sleep(100);
            }
            SimConnectHelper.SimConnectHelper.CancelRequest(variable);
            SimConnectHelper.SimConnectHelper.Disconnect();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEverySimVar()
        {
            List<SimConnectVariable> failures = new List<SimConnectVariable>();
            const int frequency = (int)SimConnectUpdateFrequency.Once;
            const int resultDelayCheckMilliseconds = 5;
            const int maxWaitForResultMilliseconds = 1000;
            SimConnectHelper.SimConnectHelper.Disconnect();
            SimConnectHelper.SimConnectHelper.SimError += SimConnect_Error;
            SimConnectHelper.SimConnectHelper.SimConnected += SimConnect_Connection;
            SimConnectHelper.SimConnectHelper.SimData += SimConnect_DataReceived;
            SimConnectHelper.SimConnectHelper.Connect();
            foreach (var simVarDefinition in SimVarUnits.DefaultUnits)
            {
                SimConnectVariable request = new SimConnectVariable { Name = simVarDefinition.Value.Name, Unit = simVarDefinition.Value.DefaultUnit };
                int requestId = SimConnectHelper.SimConnectHelper.GetSimVar(request, frequency);
                // -1 is the default value for a request that could not be sent - usually because SimConnect is not connected to MSFS 2020
                Assert.AreNotEqual(-1, requestId);
                // Ask SimConnect to fetch the latest value
                result = null;
                SimConnectHelper.SimConnectHelper.GetSimVar(requestId);
                var endWaitTime = DateTime.Now.AddMilliseconds(maxWaitForResultMilliseconds);
                while (result == null && endWaitTime > DateTime.Now)
                {
                    Thread.Sleep(resultDelayCheckMilliseconds); // Wait to receive the value
                }
                if(result == null)
                {
                    failures.Add(request);
                }
                SimConnectHelper.SimConnectHelper.CancelRequest(request);
            }
            SimConnectHelper.SimConnectHelper.Disconnect();
            foreach (var request in failures)
                Debug.WriteLine(string.Format("{0} ({1})", request.Name, request.Unit));
            Assert.AreEqual(0, failures.Count());
        }

        private void SimConnect_DataReceived(object sender, SimConnectVariableValue e)
        {
            result = e;
        }

        private void SimConnect_Connection(object sender, bool e)
        {
            Assert.IsTrue(e);
        }

        private void SimConnect_Error(object sender, ExternalException e)
        {
            throw new NotImplementedException();
        }

        private EndPoint GetEndPoint()
        {
            var ipAddress = Dns.GetHostAddresses(MSFSServerName).FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            return new IPEndPoint(ipAddress, MSFSServerPort);
        }
    }
}
