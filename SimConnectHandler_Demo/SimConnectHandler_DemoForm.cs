using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimConnectHelper;
using SimConnectHelper.Common;

namespace SimConnectHandler_DemoForm

{
    public partial class SimConnectHandler_DemoForm : Form
    {
        public SimConnectHandler_DemoForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {

        }

        private void pbConnect_Click(object sender, EventArgs e)
        {
            var server = txtSimConnectServer.Text;
            var port = (int)txtSimConnectPort.Value;
            IPAddress ipAddr = Dns.GetHostAddresses(server).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            EndPoint ep = new IPEndPoint(ipAddr, port);
            SimConnectHandler.Connect(ep);
            SimConnectHandler.SimConnected += SimConnected;
            SimConnectHandler.SimError += SimError;
            SimConnectHandler.SimData += SimData;
        }

        private void SimData(object sender, SimConnectVariableValue e)
        {
            throw new NotImplementedException();
        }

        private void SimError(object sender, IOException e)
        {
            throw e;
        }

        private void SimConnected(object sender, bool isConnected)
        {
            if (cbConnected.InvokeRequired)
            {
                cbConnected.Invoke(new Action(() => cbConnected.Checked = isConnected));
                return;
            }
            cbConnected.Checked = isConnected;
        }
    }
}
