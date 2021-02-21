using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimConnectHelper;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

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
        public void ConnectIP_Test()
        {
            SimConnectHandler.Connect(GetEndPoint());
            Assert.IsTrue(SimConnectHandler.FSConnected);
        }

        private EndPoint GetEndPoint()
        {
            var ipAddress = Dns.GetHostAddresses(MSFSServerName).FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            return new IPEndPoint(ipAddress, MSFSServerPort);
        }
    }
}
