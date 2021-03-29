
namespace SimConnectHandler_DemoForm
{
    partial class SimConnectHandler_DemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblSimConnectServer = new System.Windows.Forms.Label();
            this.txtSimConnectServer = new System.Windows.Forms.TextBox();
            this.lblSimConnectPort = new System.Windows.Forms.Label();
            this.txtSimConnectPort = new System.Windows.Forms.NumericUpDown();
            this.pbConnect = new System.Windows.Forms.Button();
            this.cbConnected = new System.Windows.Forms.CheckBox();
            this.gpConnect = new System.Windows.Forms.GroupBox();
            this.gpRequest = new System.Windows.Forms.GroupBox();
            this.lblMilliseconds = new System.Windows.Forms.Label();
            this.txtMilliseconds = new System.Windows.Forms.NumericUpDown();
            this.cmbFrequency = new System.Windows.Forms.ComboBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.cbReadOnly = new System.Windows.Forms.CheckBox();
            this.txtSimVarValue = new System.Windows.Forms.TextBox();
            this.lblSimVarValue = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.pbSendRequest = new System.Windows.Forms.Button();
            this.cmbVariable = new System.Windows.Forms.ComboBox();
            this.lblVariable = new System.Windows.Forms.Label();
            this.dgVariables = new System.Windows.Forms.DataGridView();
            this.ReqID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarFreq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VarIsReadOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SimVarUpdate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.SimVarDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cbDisableAI = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtSimConnectPort)).BeginInit();
            this.gpConnect.SuspendLayout();
            this.gpRequest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMilliseconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgVariables)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSimConnectServer
            // 
            this.lblSimConnectServer.AutoSize = true;
            this.lblSimConnectServer.Location = new System.Drawing.Point(9, 25);
            this.lblSimConnectServer.Name = "lblSimConnectServer";
            this.lblSimConnectServer.Size = new System.Drawing.Size(84, 15);
            this.lblSimConnectServer.TabIndex = 0;
            this.lblSimConnectServer.Text = "MSFS 2020 PC:";
            // 
            // txtSimConnectServer
            // 
            this.txtSimConnectServer.Location = new System.Drawing.Point(99, 22);
            this.txtSimConnectServer.Name = "txtSimConnectServer";
            this.txtSimConnectServer.Size = new System.Drawing.Size(120, 23);
            this.txtSimConnectServer.TabIndex = 1;
            this.txtSimConnectServer.Text = "localhost";
            // 
            // lblSimConnectPort
            // 
            this.lblSimConnectPort.AutoSize = true;
            this.lblSimConnectPort.Location = new System.Drawing.Point(61, 51);
            this.lblSimConnectPort.Name = "lblSimConnectPort";
            this.lblSimConnectPort.Size = new System.Drawing.Size(32, 15);
            this.lblSimConnectPort.TabIndex = 2;
            this.lblSimConnectPort.Text = "Port:";
            // 
            // txtSimConnectPort
            // 
            this.txtSimConnectPort.Location = new System.Drawing.Point(99, 51);
            this.txtSimConnectPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtSimConnectPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSimConnectPort.Name = "txtSimConnectPort";
            this.txtSimConnectPort.Size = new System.Drawing.Size(120, 23);
            this.txtSimConnectPort.TabIndex = 3;
            this.txtSimConnectPort.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // pbConnect
            // 
            this.pbConnect.Location = new System.Drawing.Point(144, 80);
            this.pbConnect.Name = "pbConnect";
            this.pbConnect.Size = new System.Drawing.Size(75, 23);
            this.pbConnect.TabIndex = 4;
            this.pbConnect.Text = "Connect";
            this.pbConnect.UseVisualStyleBackColor = true;
            this.pbConnect.Click += new System.EventHandler(this.pbConnect_Click);
            // 
            // cbConnected
            // 
            this.cbConnected.AutoSize = true;
            this.cbConnected.Enabled = false;
            this.cbConnected.Location = new System.Drawing.Point(253, 37);
            this.cbConnected.Name = "cbConnected";
            this.cbConnected.Size = new System.Drawing.Size(162, 19);
            this.cbConnected.TabIndex = 5;
            this.cbConnected.Text = "Connected to MSFS 2020?";
            this.cbConnected.UseVisualStyleBackColor = true;
            // 
            // gpConnect
            // 
            this.gpConnect.Controls.Add(this.txtSimConnectServer);
            this.gpConnect.Controls.Add(this.lblSimConnectServer);
            this.gpConnect.Controls.Add(this.pbConnect);
            this.gpConnect.Controls.Add(this.lblSimConnectPort);
            this.gpConnect.Controls.Add(this.txtSimConnectPort);
            this.gpConnect.Location = new System.Drawing.Point(12, 12);
            this.gpConnect.Name = "gpConnect";
            this.gpConnect.Size = new System.Drawing.Size(228, 117);
            this.gpConnect.TabIndex = 6;
            this.gpConnect.TabStop = false;
            this.gpConnect.Text = "Connect";
            // 
            // gpRequest
            // 
            this.gpRequest.Controls.Add(this.cbDisableAI);
            this.gpRequest.Controls.Add(this.lblMilliseconds);
            this.gpRequest.Controls.Add(this.txtMilliseconds);
            this.gpRequest.Controls.Add(this.cmbFrequency);
            this.gpRequest.Controls.Add(this.lblFrequency);
            this.gpRequest.Controls.Add(this.txtDescription);
            this.gpRequest.Controls.Add(this.lblDescription);
            this.gpRequest.Controls.Add(this.cbReadOnly);
            this.gpRequest.Controls.Add(this.txtSimVarValue);
            this.gpRequest.Controls.Add(this.lblSimVarValue);
            this.gpRequest.Controls.Add(this.txtUnit);
            this.gpRequest.Controls.Add(this.lblUnit);
            this.gpRequest.Controls.Add(this.pbSendRequest);
            this.gpRequest.Controls.Add(this.cmbVariable);
            this.gpRequest.Controls.Add(this.lblVariable);
            this.gpRequest.Location = new System.Drawing.Point(13, 136);
            this.gpRequest.Name = "gpRequest";
            this.gpRequest.Size = new System.Drawing.Size(402, 178);
            this.gpRequest.TabIndex = 7;
            this.gpRequest.TabStop = false;
            this.gpRequest.Text = "Request";
            // 
            // lblMilliseconds
            // 
            this.lblMilliseconds.AutoSize = true;
            this.lblMilliseconds.Location = new System.Drawing.Point(273, 106);
            this.lblMilliseconds.Name = "lblMilliseconds";
            this.lblMilliseconds.Size = new System.Drawing.Size(73, 15);
            this.lblMilliseconds.TabIndex = 13;
            this.lblMilliseconds.Text = "milliseconds";
            // 
            // txtMilliseconds
            // 
            this.txtMilliseconds.Enabled = false;
            this.txtMilliseconds.Location = new System.Drawing.Point(192, 103);
            this.txtMilliseconds.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.txtMilliseconds.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.txtMilliseconds.Name = "txtMilliseconds";
            this.txtMilliseconds.Size = new System.Drawing.Size(74, 23);
            this.txtMilliseconds.TabIndex = 12;
            this.txtMilliseconds.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // cmbFrequency
            // 
            this.cmbFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrequency.FormattingEnabled = true;
            this.cmbFrequency.Location = new System.Drawing.Point(65, 103);
            this.cmbFrequency.Name = "cmbFrequency";
            this.cmbFrequency.Size = new System.Drawing.Size(121, 23);
            this.cmbFrequency.TabIndex = 11;
            this.cmbFrequency.SelectedIndexChanged += new System.EventHandler(this.Frequency_Changed);
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(25, 106);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(33, 15);
            this.lblFrequency.TabIndex = 10;
            this.lblFrequency.Text = "Freq:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(65, 46);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(310, 23);
            this.txtDescription.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(24, 51);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(35, 15);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Desc:";
            // 
            // cbReadOnly
            // 
            this.cbReadOnly.AutoSize = true;
            this.cbReadOnly.Enabled = false;
            this.cbReadOnly.Location = new System.Drawing.Point(179, 132);
            this.cbReadOnly.Name = "cbReadOnly";
            this.cbReadOnly.Size = new System.Drawing.Size(85, 19);
            this.cbReadOnly.TabIndex = 7;
            this.cbReadOnly.Text = "Read Only?";
            this.cbReadOnly.UseVisualStyleBackColor = true;
            // 
            // txtSimVarValue
            // 
            this.txtSimVarValue.Enabled = false;
            this.txtSimVarValue.Location = new System.Drawing.Point(65, 137);
            this.txtSimVarValue.Name = "txtSimVarValue";
            this.txtSimVarValue.Size = new System.Drawing.Size(107, 23);
            this.txtSimVarValue.TabIndex = 6;
            // 
            // lblSimVarValue
            // 
            this.lblSimVarValue.AutoSize = true;
            this.lblSimVarValue.Location = new System.Drawing.Point(21, 140);
            this.lblSimVarValue.Name = "lblSimVarValue";
            this.lblSimVarValue.Size = new System.Drawing.Size(38, 15);
            this.lblSimVarValue.TabIndex = 5;
            this.lblSimVarValue.Text = "Value:";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(65, 74);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(310, 23);
            this.txtUnit.TabIndex = 4;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(27, 78);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(32, 15);
            this.lblUnit.TabIndex = 3;
            this.lblUnit.Text = "Unit:";
            // 
            // pbSendRequest
            // 
            this.pbSendRequest.Location = new System.Drawing.Point(288, 149);
            this.pbSendRequest.Name = "pbSendRequest";
            this.pbSendRequest.Size = new System.Drawing.Size(108, 23);
            this.pbSendRequest.TabIndex = 2;
            this.pbSendRequest.Text = "Send Request";
            this.pbSendRequest.UseVisualStyleBackColor = true;
            this.pbSendRequest.Click += new System.EventHandler(this.pbSendRequest_Click);
            // 
            // cmbVariable
            // 
            this.cmbVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVariable.FormattingEnabled = true;
            this.cmbVariable.Location = new System.Drawing.Point(65, 18);
            this.cmbVariable.Name = "cmbVariable";
            this.cmbVariable.Size = new System.Drawing.Size(310, 23);
            this.cmbVariable.TabIndex = 1;
            this.cmbVariable.SelectedIndexChanged += new System.EventHandler(this.Variable_Changed);
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Location = new System.Drawing.Point(8, 21);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(51, 15);
            this.lblVariable.TabIndex = 0;
            this.lblVariable.Text = "Variable:";
            // 
            // dgVariables
            // 
            this.dgVariables.AllowUserToAddRows = false;
            this.dgVariables.AllowUserToDeleteRows = false;
            this.dgVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReqID,
            this.SimVarName,
            this.SimVarUnit,
            this.SimVarFreq,
            this.SimVarValue,
            this.VarIsReadOnly,
            this.SimVarUpdate,
            this.SimVarDelete});
            this.dgVariables.Location = new System.Drawing.Point(427, 12);
            this.dgVariables.MultiSelect = false;
            this.dgVariables.Name = "dgVariables";
            this.dgVariables.ReadOnly = true;
            this.dgVariables.RowHeadersVisible = false;
            this.dgVariables.RowTemplate.Height = 25;
            this.dgVariables.Size = new System.Drawing.Size(593, 486);
            this.dgVariables.TabIndex = 8;
            this.dgVariables.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvButton_Click);
            // 
            // ReqID
            // 
            this.ReqID.HeaderText = "ReqID";
            this.ReqID.Name = "ReqID";
            this.ReqID.ReadOnly = true;
            this.ReqID.Visible = false;
            this.ReqID.Width = 80;
            // 
            // SimVarName
            // 
            this.SimVarName.HeaderText = "SimVar";
            this.SimVarName.Name = "SimVarName";
            this.SimVarName.ReadOnly = true;
            this.SimVarName.Width = 200;
            // 
            // SimVarUnit
            // 
            this.SimVarUnit.HeaderText = "Unit";
            this.SimVarUnit.Name = "SimVarUnit";
            this.SimVarUnit.ReadOnly = true;
            this.SimVarUnit.Visible = false;
            this.SimVarUnit.Width = 150;
            // 
            // SimVarFreq
            // 
            this.SimVarFreq.HeaderText = "Frequency";
            this.SimVarFreq.Name = "SimVarFreq";
            this.SimVarFreq.ReadOnly = true;
            this.SimVarFreq.Width = 80;
            // 
            // SimVarValue
            // 
            this.SimVarValue.HeaderText = "Value";
            this.SimVarValue.Name = "SimVarValue";
            this.SimVarValue.ReadOnly = true;
            // 
            // VarIsReadOnly
            // 
            this.VarIsReadOnly.HeaderText = "Read Only";
            this.VarIsReadOnly.Name = "VarIsReadOnly";
            this.VarIsReadOnly.ReadOnly = true;
            this.VarIsReadOnly.Width = 60;
            // 
            // SimVarUpdate
            // 
            this.SimVarUpdate.HeaderText = "Update";
            this.SimVarUpdate.Name = "SimVarUpdate";
            this.SimVarUpdate.ReadOnly = true;
            this.SimVarUpdate.Text = "Update";
            this.SimVarUpdate.ToolTipText = "Update";
            this.SimVarUpdate.UseColumnTextForButtonValue = true;
            this.SimVarUpdate.Width = 50;
            // 
            // SimVarDelete
            // 
            this.SimVarDelete.HeaderText = "Delete";
            this.SimVarDelete.Name = "SimVarDelete";
            this.SimVarDelete.ReadOnly = true;
            this.SimVarDelete.Text = "Delete";
            this.SimVarDelete.ToolTipText = "Delete";
            this.SimVarDelete.UseColumnTextForButtonValue = true;
            this.SimVarDelete.Width = 50;
            // 
            // txtErrors
            // 
            this.txtErrors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtErrors.ForeColor = System.Drawing.Color.Red;
            this.txtErrors.Location = new System.Drawing.Point(12, 320);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrors.Size = new System.Drawing.Size(403, 178);
            this.txtErrors.TabIndex = 9;
            this.txtErrors.WordWrap = false;
            // 
            // cbDisableAI
            // 
            this.cbDisableAI.AutoSize = true;
            this.cbDisableAI.Location = new System.Drawing.Point(179, 149);
            this.cbDisableAI.Name = "cbDisableAI";
            this.cbDisableAI.Size = new System.Drawing.Size(78, 19);
            this.cbDisableAI.TabIndex = 14;
            this.cbDisableAI.Text = "Disable AI";
            this.cbDisableAI.UseVisualStyleBackColor = true;
            // 
            // SimConnectHandler_DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 510);
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.dgVariables);
            this.Controls.Add(this.gpRequest);
            this.Controls.Add(this.gpConnect);
            this.Controls.Add(this.cbConnected);
            this.Name = "SimConnectHandler_DemoForm";
            this.Text = "SimConnect Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClose_Click);
            ((System.ComponentModel.ISupportInitialize)(this.txtSimConnectPort)).EndInit();
            this.gpConnect.ResumeLayout(false);
            this.gpConnect.PerformLayout();
            this.gpRequest.ResumeLayout(false);
            this.gpRequest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMilliseconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgVariables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSimConnectServer;
        private System.Windows.Forms.TextBox txtSimConnectServer;
        private System.Windows.Forms.Label lblSimConnectPort;
        private System.Windows.Forms.NumericUpDown txtSimConnectPort;
        private System.Windows.Forms.Button pbConnect;
        private System.Windows.Forms.CheckBox cbConnected;
        private System.Windows.Forms.GroupBox gpConnect;
        private System.Windows.Forms.GroupBox gpRequest;
        private System.Windows.Forms.Button pbSendRequest;
        private System.Windows.Forms.ComboBox cmbVariable;
        private System.Windows.Forms.Label lblVariable;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.DataGridView dgVariables;
        private System.Windows.Forms.TextBox txtSimVarValue;
        private System.Windows.Forms.Label lblSimVarValue;
        private System.Windows.Forms.CheckBox cbReadOnly;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox cmbFrequency;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label lblMilliseconds;
        private System.Windows.Forms.NumericUpDown txtMilliseconds;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReqID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarFreq;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn VarIsReadOnly;
        private System.Windows.Forms.DataGridViewButtonColumn SimVarUpdate;
        private System.Windows.Forms.DataGridViewButtonColumn SimVarDelete;
        private System.Windows.Forms.CheckBox cbDisableAI;
    }
}