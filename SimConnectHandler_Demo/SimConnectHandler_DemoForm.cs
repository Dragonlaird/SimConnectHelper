using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SimConnectHandler_Demo;
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
            FARyoufly_Sample sample = new FARyoufly_Sample();
            sample.Main();
        }

        private void Initialize()
        {
            var dataSource = simVarVariable.OrderBy(x=> x.Key).ToList();
            cmbVariable.DataSource = dataSource; //.SimVarList().ToList();
            cmbVariable.DisplayMember = "Key";
            cmbVariable.ValueMember = "Key";
            dgVariables.Rows.Clear();
            cbReadOnly.Checked = simVarVariable.OrderBy(x => x.Key).First().Value.ReadOnly;
            txtErrors.ReadOnly = true;
            cmbFrequency.DataSource = null;
            var cmbDataSource = new List<string>();
            foreach(var freq in Enum.GetValues(typeof(SimConnectUpdateFrequency)).Cast<SimConnectUpdateFrequency>())
            {
                cmbDataSource.Add(freq.ToString());
            }
            cmbDataSource.Add("Milliseconds");
            cmbFrequency.DataSource = cmbDataSource;
        }

        private void SetInvocations()
        {
            if (SimConnectHelper.SimConnectHelper.SimConnected == null || SimConnectHelper.SimConnectHelper.SimConnected.GetInvocationList().Length == 0)
                SimConnectHelper.SimConnectHelper.SimConnected += SimConnected;
            if (SimConnectHelper.SimConnectHelper.SimError == null || SimConnectHelper.SimConnectHelper.SimError.GetInvocationList().Length == 0)
                global::SimConnectHelper.SimConnectHelper.SimError += SimError;
            if (SimConnectHelper.SimConnectHelper.SimData == null || SimConnectHelper.SimConnectHelper.SimData.GetInvocationList().Length == 0)
                SimConnectHelper.SimConnectHelper.SimData += SimData;
            if (SimConnectHelper.SimConnectHelper.SimLog == null || SimConnectHelper.SimConnectHelper.SimLog.GetInvocationList().Length == 0)
                SimConnectHelper.SimConnectHelper.SimLog += SimLog;
        }

        private void pbConnect_Click(object sender, EventArgs e)
        {
            SetInvocations();
            if (!SimConnectHelper.SimConnectHelper.IsConnected)
            {
                var server = txtSimConnectServer.Text;
                var port = (int)txtSimConnectPort.Value;
                IPAddress ipAddr = Dns.GetHostAddresses(server).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                EndPoint ep = new IPEndPoint(ipAddr, port);
                SimConnectHelper.SimConnectHelper.Connect(ep);
            }
            else
            {
                SimConnectHelper.SimConnectHelper.Disconnect();
            }
        }

        private void SimLog(object sender, SimVarMessage e)
        {
            UpdateErrorText(sender, string.Format("[{0}] {1}", e.Severity.ToString().Substring(0, 3), e.Message));
        }

        private void SimData(object sender, SimConnectVariableValue e)
        {
            if (e.Value != null)
            {
                var row = FindRowBySimVarName(e.Request.Name);
                if (row != null)
                {
                    var value = e.Value.GetType().IsArray ? ((object[])e.Value)[0].ToString() : e.Value.ToString();
                    row.Cells["SimVarValue"].Value = value;
                }
            }
        }

        /// <summary>
        /// Called whenever SimConnect generates an error
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="e">Exception containing SimConnect error data</param>
        private void SimError(object sender, ExternalException e)
        {
            var errorId = Convert.ToInt32(e.Data["dwID"]);
            var sendId = Convert.ToInt32(e.Data["dwSendID"]);
            var indexId = Convert.ToDouble(e.Data["dwIndex"]);
            var exceptionId = Convert.ToInt32(e.Data["dwException"]);
            var exceptionType = (string)e.Data["exceptionType"];
            var errorMessage = string.Format("\terrorId: {0}\r\n\tsendId: {1}\r\n\tindexId: {2}\r\n\texceptionId: {3}\r\n\texceptionType: {4}", errorId, sendId, indexId, exceptionId, exceptionType);
            UpdateErrorText(txtErrors, string.Format("\r\n{0:HH:mm:ss} ({1}) {2}\r\nData:\r\n{3}", DateTime.Now, exceptionType, e.Message, errorMessage));
            //throw e;
        }

        private void UpdateErrorText(object sender, string text)
        {
            if (txtErrors.InvokeRequired)
            {
                txtErrors.Invoke(new Action(() => UpdateErrorText(sender, text)));
                return;
            }
            txtErrors.Text += string.Format("{0}\r\n", text);
            while (txtErrors.Text.Count(x => x == '\r') > 100)
                txtErrors.Text = txtErrors.Text.Substring(txtErrors.Text.IndexOf('\r') + 2);
            txtErrors.SelectionStart = txtErrors.Text.Length;
            txtErrors.ScrollToCaret();
            txtErrors.Update();
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
                SimConnectHelper.SimConnectHelper.GetSimVar(request);
            }
        }

        private void Variable_Changed(object sender, EventArgs e)
        {
            var simVar = (KeyValuePair<string, SimVarDefinition>)cmbVariable.SelectedItem;
            txtDescription.Text = simVar.Value.Description;
            toolTip.SetToolTip(txtDescription, txtDescription.Text);
            txtUnit.Text = simVar.Value.DefaultUnit;
            txtSimVarValue.Text = "";
            //if (simVar.Value.ReadOnly)
            //    txtSimVarValue.Enabled = false;
            //else
            txtSimVarValue.Enabled = true;
            cbReadOnly.Checked = simVar.Value.ReadOnly;
        }

        private DataGridViewRow FindRowBySimVarName(string simVarName)
        {
            lock (dgVariables.Rows)
                try
                {
                    foreach (DataGridViewRow row in dgVariables.Rows)
                        if (row.Cells["SimVarName"].Value?.ToString() == simVarName)
                            return row;
                }
                catch { } // If rows are updated whilst we were iterating through the collection, ignore it
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
                var value = "";
                int reqId = -1;
                //var isReadOnly = simVarDefinition.ReadOnly;
                //if (!simVarDefinition.ReadOnly)
                value = txtSimVarValue.Text;
                var frequency = cmbFrequency.SelectedItem;
                if (frequency.ToString() == "Milliseconds")
                    frequency = (int)txtMilliseconds.Value;
                int rowIdx = dgVariables.Rows.Add(new object[]
                {
                    0, // RecID
                    simVarName, // SimVar
                    simVarDefinition.DefaultUnit, // Units
                    frequency.ToString(), // Frequency
                    value, // Value
                    simVarDefinition.ReadOnly // ReadOnly
                });
                dgVariables.Rows[rowIdx].Cells["ReqID"].ReadOnly = true;
                dgVariables.Rows[rowIdx].Cells["SimVarName"].ReadOnly = true;
                dgVariables.Rows[rowIdx].Cells["SimVarUnit"].ReadOnly = true;
                dgVariables.Rows[rowIdx].Cells["SimVarFreq"].ReadOnly = true;
                dgVariables.Rows[rowIdx].Cells["VarIsReadOnly"].ReadOnly = true;
                dgVariables.Rows[rowIdx].Cells["SimVarValue"].ReadOnly = false;
                //ReqID
                //SimVarName
                //SimVarUnit
                //SimVarValue
                //VarIsReadOnly
                SimConnectVariable variableRequest = new SimConnectVariable
                {
                    Name = simVarName,
                    Unit = simVarDefinition.DefaultUnit
                };
                if (string.IsNullOrEmpty(value))
                {
                    // Send Request - then update ReqID cell with returned request ID
                    reqId = SendRequest(variableRequest, (int)Enum.Parse(typeof(SimConnectUpdateFrequency),frequency.ToString()));
                }
                else
                {
                    SimConnectVariableValue variableValue = new SimConnectVariableValue
                    {
                        Request = variableRequest,
                        Value = value
                    };
                    reqId = SendValue(variableValue, cbDisableAI.Checked);
                }
                dgVariables.Rows[rowIdx].Cells["ReqID"].Value = reqId;
            }
        }

        private void dgvButton_Click(object sender, DataGridViewCellEventArgs e)
        {
            var reqId = (int?)dgVariables.Rows[e.RowIndex].Cells["ReqID"].Value;
            if (e.ColumnIndex == dgVariables.Columns["SimVarUpdate"].Index)
            {
                // User wants to refresh the displayed value
                if (reqId > -1)
                    SimConnectHelper.SimConnectHelper.GetSimVar((int)reqId);
                else
                {
                    var isReadOnly = ((DataGridViewCheckBoxCell)dgVariables.Rows[e.RowIndex].Cells["VarIsReadOnly"]).Value
                        == ((DataGridViewCheckBoxCell)dgVariables.Rows[e.RowIndex].Cells["VarIsReadOnly"]).TrueValue;
                    var simVarName = (string)dgVariables.Rows[e.RowIndex].Cells["SimVarName"].Value;
                    var simVarUnit = (string)dgVariables.Rows[e.RowIndex].Cells["SimVarUnit"].Value;
                    var frequency = dgVariables.Rows[e.RowIndex].Cells["SimVarFreq"].Value;
                    SimConnectVariable request = new SimConnectVariable
                    {
                        Name = simVarName,
                        Unit = simVarUnit
                    };
                    if (isReadOnly)
                        dgVariables.Rows[e.RowIndex].Cells["ReqID"].Value = SendRequest(request, int.Parse(frequency.ToString()));
                    else
                    {
                        var value = dgVariables.Rows[e.RowIndex].Cells["SimVarValue"].Value;
                        SendValue(new SimConnectVariableValue
                        {
                            Request = request,
                            Value = value
                        }, cbDisableAI.Checked);
                    }
                }
            }
            if (e.ColumnIndex == dgVariables.Columns["SimVarDelete"].Index)
            {
                var simVarName = (string)dgVariables.Rows[e.RowIndex].Cells["SimVarName"].Value;
                var simVarUnit = (string)dgVariables.Rows[e.RowIndex].Cells["SimVarUnit"].Value;
                SimConnectVariable request = new SimConnectVariable
                {
                    Name = simVarName,
                    Unit = simVarUnit
                };
                SimConnectHelper.SimConnectHelper.CancelRequest(request);
                dgVariables.Rows.RemoveAt(e.RowIndex);
            }
        }

        private int SendValue(SimConnectVariableValue variableValue, bool disableAI = false)
        {
            return SimConnectHelper.SimConnectHelper.SetSimVar(variableValue, disableAI);
        }

        private int SendRequest(SimConnectVariable request, int frequency)
        {
            return SimConnectHelper.SimConnectHelper.GetSimVar(request, frequency); // If FetchLatestValue = true; Auto-update
        }

        private void FormClose_Click(object sender, FormClosingEventArgs e)
        {
            SimConnectHelper.SimConnectHelper.Disconnect();
        }

        private void Frequency_Changed(object sender, EventArgs e)
        {
            if (cmbFrequency.SelectedItem.ToString() == "Milliseconds")
                txtMilliseconds.Enabled = true;
            else
                txtMilliseconds.Enabled = false;
        }
    }
}
