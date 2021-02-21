using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimConnectHelper;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SimConnectHandler_Tests
{
    [TestClass]
    public class SimConnectHandler_Tests
    {
        private const string MSFSServerName = "localhost";
        private const int MSFSServerPort = 500;
        /// <summary>
        /// Connect_Test will only pass if MSFS 2020 is running locally
        /// </summary>
        [TestMethod]
        public void Connect_Test()
        {
            SimConnectHandler.Connect();
            Assert.IsTrue(SimConnectHandler.FSConnected);
        }

        /// <summary>
        /// ConnectIP_Test will only pass if MSFS 2020 is running on the remote IP (and the firewall on that PC permits access)
        /// </summary>
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
