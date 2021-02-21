using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimConnectHelper;
using SimConnectHelper.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

        [TestMethod]
        public void Connect_Test()
        {
            SimConnectHandler.Connect();
            Assert.IsTrue(SimConnectHandler.FSConnected);
        }

        [TestMethod]
        public void ConnectViaIP_Test()
        {
            SimConnectHandler.Connect(GetEndPoint());
            Assert.IsTrue(SimConnectHandler.FSConnected);
        }

        [TestMethod]
        public void RequestSimVar_Test()
        {
            SimConnectHandler.SimError += SimConnect_Error;
            SimConnectHandler.SimConnected += SimConnect_Connection;
            SimConnectHandler.SimData += SimConnect_DataReceived;
            SimConnectHandler.Connect();
            var variable = new SimConnectVariable
            {
                Name = "TITLE",
                Unit = "string"
            };
            SimConnectHandler.SendRequest(variable);
            Thread.Sleep(1000);

        }

        private void SimConnect_DataReceived(object sender, SimConnectVariableValue e)
        {
            throw new NotImplementedException();
        }

        private void SimConnect_Connection(object sender, bool e)
        {
            Assert.IsTrue(e);
        }

        private void SimConnect_Error(object sender, IOException e)
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
