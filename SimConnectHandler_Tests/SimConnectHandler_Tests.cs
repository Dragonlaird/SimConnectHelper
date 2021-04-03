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

namespace SimConnectHandler_Tests
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
            SimConnectHandler.Connect(); // Find/Try all defined server connection configurations
            Assert.IsTrue(SimConnectHandler.IsConnected);
        }

        [TestMethod]
        public void Disconnect_Test()
        {
            SimConnectHandler.Connect(); // Find/Try all defined server connection configurations
            Assert.IsTrue(SimConnectHandler.IsConnected);
            SimConnectHandler.Disconnect();
            Assert.IsFalse(SimConnectHandler.IsConnected);
        }

        [TestMethod]
        public void ConnectConfiguration_Test()
        {
            SimConnectHandler.Connect(); // Find/Try all defined server connection configurations
            Assert.AreNotEqual(SimConnectHandler.Connection.Port, "0");
        }

        [TestMethod]
        public void ConnectViaIP_Test()
        {
            SimConnectHandler.Connect(GetEndPoint());
            Thread.Sleep(1000);
            Assert.IsTrue(SimConnectHandler.IsConnected);
        }

        [TestMethod]
        public void RequestSimVar_Test_In_Ms()
        {
            result = null;
            SimConnectHandler.SimError += SimConnect_Error;
            SimConnectHandler.SimConnected += SimConnect_Connection;
            SimConnectHandler.SimData += SimConnect_DataReceived;
            SimConnectHandler.Connect();
            var variable = new SimConnectVariable
            {
                Name = "AMBIENT WIND VELOCITY",
                Unit = "knots"
            };
            var requestID = SimConnectHandler.GetSimVar(variable, 50);

            // Wait up to 5 seconds for MSFS to return the requested value
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (result == null && DateTime.Now < endTime)
            {
                Thread.Sleep(100);
            }
            SimConnectHandler.CancelRequest(variable);
            SimConnectHandler.Disconnect();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void RequestSimVar_Test_Once()
        {
            result = null;
            SimConnectHandler.SimError += SimConnect_Error;
            SimConnectHandler.SimConnected += SimConnect_Connection;
            SimConnectHandler.SimData += SimConnect_DataReceived;
            SimConnectHandler.Connect();
            var variable = new SimConnectVariable
            {
                Name = "AMBIENT WIND VELOCITY",
                Unit = "knots"
            };
            var requestID = SimConnectHandler.RegisterSimVar(variable, SimConnectUpdateFrequency.Once);

            // Wait up to 5 seconds for MSFS to return the requested value
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (result == null && DateTime.Now < endTime)
            {
                Thread.Sleep(100);
            }
            SimConnectHandler.CancelRequest(variable);
            SimConnectHandler.Disconnect();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RequestSimVar_Test_SimFrame()
        {
            result = null;
            SimConnectHandler.SimError += SimConnect_Error;
            SimConnectHandler.SimConnected += SimConnect_Connection;
            SimConnectHandler.SimData += SimConnect_DataReceived;
            SimConnectHandler.Connect();
            var variable = new SimConnectVariable
            {
                Name = "AMBIENT WIND VELOCITY",
                Unit = "knots"
            };
            var requestID = SimConnectHandler.RegisterSimVar(variable, SimConnectUpdateFrequency.SIM_Frame);

            // Wait up to 5 seconds for MSFS to return the requested value
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (result == null && DateTime.Now < endTime)
            {
                Thread.Sleep(100);
            }
            SimConnectHandler.CancelRequest(variable);
            SimConnectHandler.Disconnect();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEverySimVar()
        {
            List<SimConnectVariable> failures = new List<SimConnectVariable>();
            const int frequency = (int)SimConnectUpdateFrequency.Once;
            const int resultDelayCheckMilliseconds = 5;
            const int maxWaitForResultMilliseconds = 1000;
            SimConnectHandler.Disconnect();
            SimConnectHandler.SimError += SimConnect_Error;
            SimConnectHandler.SimConnected += SimConnect_Connection;
            SimConnectHandler.SimData += SimConnect_DataReceived;
            SimConnectHandler.Connect();
            foreach (var simVarDefinition in SimVarUnits.DefaultUnits)
            {
                SimConnectVariable request = new SimConnectVariable { Name = simVarDefinition.Value.Name, Unit = simVarDefinition.Value.DefaultUnit };
                int requestId = SimConnectHandler.GetSimVar(request, frequency);
                // -1 is the default value for a request that could not be sent - usually because SimConnect is not connected to MSFS 2020
                Assert.AreNotEqual(-1, requestId);
                // Ask SimConnect to fetch the latest value
                result = null;
                SimConnectHandler.GetSimVar(requestId);
                var endWaitTime = DateTime.Now.AddMilliseconds(maxWaitForResultMilliseconds);
                while (result == null && endWaitTime > DateTime.Now)
                {
                    Thread.Sleep(resultDelayCheckMilliseconds); // Wait to receive the value
                }
                if(result == null)
                {
                    failures.Add(request);
                }
                SimConnectHandler.CancelRequest(request);
            }
            SimConnectHandler.Disconnect();
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
            //Assert.IsTrue(e);
        }

        private void SimConnect_Error(object sender, ExternalException e)
        {
            //throw new NotImplementedException();
        }

        private EndPoint GetEndPoint()
        {
            var ipAddress = Dns.GetHostAddresses(MSFSServerName).FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            return new IPEndPoint(ipAddress, MSFSServerPort);
        }
    }
}
