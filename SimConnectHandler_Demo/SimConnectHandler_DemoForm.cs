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
            cmbVariable.DataSource = new SimConnectHelper.Common.SimVarList().ToList();
            cmbVariable.DisplayMember = "Key";
            cmbVariable.ValueMember = "Value";
            dgVariables.Rows.Clear();
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
            var row = FindRowBySimVarName(e.Request.Name);
            if (row != null)
                if (e?.Value?.GetType().IsArray == true)
                    row.Cells["SimVarValue"].Value = ((object[])e.Value)[0].ToString();
                else
                    row.Cells["SimVarValue"].Value = e.Value?.ToString();
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
            if (isConnected)
                // Re-request all SimVar values for a new ReqID
                RequestAllSimVars();
        }

        private void RequestAllSimVars()
        {
            foreach (DataGridViewRow row in dgVariables.Rows)
            {
                var simVarName = row.Cells["SimVarName"].Value?.ToString();
                var simVarUnit = row.Cells["SimVarValue"].Value?.ToString();
                var request = new SimConnectVariable
                {
                    Name = simVarName,
                    Unit = simVarUnit
                };
                SimConnectHandler.SendRequest(request, true);
            }
        }

        private void Variable_Changed(object sender, EventArgs e)
        {
            txtUnit.Text = ((KeyValuePair<string,string>)cmbVariable.SelectedItem).Value;
        }

        private DataGridViewRow FindRowBySimVarName(string simVarName)
        {
            foreach (DataGridViewRow row in dgVariables.Rows)
            {
                if (row.Cells["SimVarName"].Value?.ToString() == simVarName)
                    return row;
            }
            return null;
        }

        private void pbSendRequest_Click(object sender, EventArgs e)
        {
            var simVarName = ((KeyValuePair<string, string>)cmbVariable.SelectedItem).Key;
            var simVarUnit = ((KeyValuePair<string, string>)cmbVariable.SelectedItem).Value;
            bool bCanSendRequest = FindRowBySimVarName(simVarName) == null;
            if (bCanSendRequest)
            {
                int rowIdx = dgVariables.Rows.Add(new object[]
                {
                    0, // RecID
                    simVarName, // SimVar
                    simVarUnit, // Units
                    "" // Value
                });
                // Send Request - then update ReqID cell with returned request ID
                SimConnectVariable variableRequest = new SimConnectVariable
                {
                    Name = simVarName,
                    Unit = simVarUnit
                };
                var reqId = SimConnectHandler.SendRequest(variableRequest, true);
                dgVariables.Rows[rowIdx].Cells["ReqID"].Value = reqId;
            }

        }

        private void Update_Click(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgVariables.Columns["Update"].Index)
            {
                // User wants to refresh the displayed value
                var reqId = (int?)dgVariables.Rows[e.RowIndex].Cells["ReqID"].Value;
                if (reqId > -1)
                    SimConnectHandler.FetchValueUpdate((int)reqId);
            }
        }
    }
}
