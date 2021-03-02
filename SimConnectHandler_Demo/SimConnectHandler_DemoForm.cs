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
        private readonly Dictionary<string, SimVarDefinition> simVarVariable = SimVarUnits.DefaultUnits;
        public SimConnectHandler_DemoForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            var dataSource = simVarVariable.OrderBy(x=> x.Key).ToList();
            cmbVariable.DataSource = dataSource; //.SimVarList().ToList();
            cmbVariable.DisplayMember = "Key";
            cmbVariable.ValueMember = "Key";
            dgVariables.Rows.Clear();
        }

        private void pbConnect_Click(object sender, EventArgs e)
        {
            if (!SimConnectHandler.FSConnected)
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
            else
            {
                SimConnectHandler.Disconnect();
            }
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

        /// <summary>
        /// Called whenever SimConnect generates an error
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="e">Exception containing SimConnect error data</param>
        private void SimError(object sender, IOException e)
        {
            //throw e;
        }

        /// <summary>
        /// Called whenever SimConnect changes connection state to MSFS 2020
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="isConnected">True = Connect; False = Disconnected</param>
        private void SimConnected(object sender, bool isConnected)
        {
            if (cbConnected.InvokeRequired)
            {
                cbConnected.Invoke(new Action(() => SimConnected(sender, isConnected)));
                return;
            }
            cbConnected.Checked = isConnected;
            if (isConnected)
            {
                // Re-request all SimVar values for a new ReqID
                pbConnect.Text = "Disconnect";
                RequestAllSimVars();
            }
            else
            {
                pbConnect.Text = "Connect";
            }
        }

        private void RequestAllSimVars()
        {
            foreach (DataGridViewRow row in dgVariables.Rows)
            {
                var simVarName = row.Cells["SimVarName"].Value?.ToString();
                var simVarUnit = row.Cells["SimVarUnit"].Value?.ToString();
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
            var simVar = (KeyValuePair<string, SimVarDefinition>)cmbVariable.SelectedItem;
            txtUnit.Text = simVar.Value.DefaultUnit;
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
            var selectedItem = (KeyValuePair<string, SimVarDefinition>)cmbVariable.SelectedItem;
            var simVarName = selectedItem.Key;
            var simVarDefinition = selectedItem.Value;
            bool bCanSendRequest = FindRowBySimVarName(simVarName) == null;
            if (bCanSendRequest)
            {
                int rowIdx = dgVariables.Rows.Add(new object[]
                {
                    0, // RecID
                    simVarName, // SimVar
                    simVarDefinition.DefaultUnit, // Units
                    "" // Value
                });
                // Send Request - then update ReqID cell with returned request ID
                SimConnectVariable variableRequest = new SimConnectVariable
                {
                    Name = simVarName,
                    Unit = simVarDefinition.DefaultUnit
                };
                var reqId = SendRequest(variableRequest, true);
                dgVariables.Rows[rowIdx].Cells["ReqID"].Value = reqId;
            }

        }

        private void Update_Click(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgVariables.Columns["SimVarUpdate"].Index)
            {
                // User wants to refresh the displayed value
                var reqId = (int?)dgVariables.Rows[e.RowIndex].Cells["ReqID"].Value;
                if (reqId > -1)
                    SimConnectHandler.GetSimVar((int)reqId);
                else
                {
                    var simVarName = (string)dgVariables.Rows[e.RowIndex].Cells["SimVarName"].Value;
                    var simVarUnit = (string)dgVariables.Rows[e.RowIndex].Cells["SimVarUnit"].Value;
                    SimConnectVariable request = new SimConnectVariable
                    {
                        Name = simVarName,
                        Unit = simVarUnit
                    };
                    dgVariables.Rows[e.RowIndex].Cells["ReqID"].Value = SendRequest(request, true);
                }
            }
        }

        private int SendRequest(SimConnectVariable request, bool FetchLatestValue = false)
        {
            return SimConnectHandler.SendRequest(request, FetchLatestValue);
        }

        private void FormClose_Click(object sender, FormClosingEventArgs e)
        {
            SimConnectHandler.Disconnect();
        }
    }
}
