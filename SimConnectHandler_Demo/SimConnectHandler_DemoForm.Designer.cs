
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
            this.lblSimConnectServer = new System.Windows.Forms.Label();
            this.txtSimConnectServer = new System.Windows.Forms.TextBox();
            this.lblSimConnectPort = new System.Windows.Forms.Label();
            this.txtSimConnectPort = new System.Windows.Forms.NumericUpDown();
            this.pbConnect = new System.Windows.Forms.Button();
            this.cbConnected = new System.Windows.Forms.CheckBox();
            this.gpConnect = new System.Windows.Forms.GroupBox();
            this.gpRequest = new System.Windows.Forms.GroupBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.pbSendRequest = new System.Windows.Forms.Button();
            this.cmbVariable = new System.Windows.Forms.ComboBox();
            this.lblVariable = new System.Windows.Forms.Label();
            this.dgVariables = new System.Windows.Forms.DataGridView();
            this.ReqID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimVarValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Update = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.txtSimConnectPort)).BeginInit();
            this.gpConnect.SuspendLayout();
            this.gpRequest.SuspendLayout();
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
            this.gpRequest.Controls.Add(this.txtUnit);
            this.gpRequest.Controls.Add(this.lblUnit);
            this.gpRequest.Controls.Add(this.pbSendRequest);
            this.gpRequest.Controls.Add(this.cmbVariable);
            this.gpRequest.Controls.Add(this.lblVariable);
            this.gpRequest.Location = new System.Drawing.Point(13, 136);
            this.gpRequest.Name = "gpRequest";
            this.gpRequest.Size = new System.Drawing.Size(381, 130);
            this.gpRequest.TabIndex = 7;
            this.gpRequest.TabStop = false;
            this.gpRequest.Text = "Request";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(65, 64);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(310, 23);
            this.txtUnit.TabIndex = 4;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(27, 68);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(32, 15);
            this.lblUnit.TabIndex = 3;
            this.lblUnit.Text = "Unit:";
            // 
            // pbSendRequest
            // 
            this.pbSendRequest.Location = new System.Drawing.Point(267, 93);
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
            this.cmbVariable.Location = new System.Drawing.Point(65, 30);
            this.cmbVariable.Name = "cmbVariable";
            this.cmbVariable.Size = new System.Drawing.Size(310, 23);
            this.cmbVariable.TabIndex = 1;
            this.cmbVariable.SelectedIndexChanged += new System.EventHandler(this.Variable_Changed);
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Location = new System.Drawing.Point(8, 33);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(51, 15);
            this.lblVariable.TabIndex = 0;
            this.lblVariable.Text = "Variable:";
            // 
            // dgVariables
            // 
            this.dgVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReqID,
            this.SimVarName,
            this.SimVarUnit,
            this.SimVarValue,
            this.Update});
            this.dgVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgVariables.Location = new System.Drawing.Point(427, 12);
            this.dgVariables.MultiSelect = false;
            this.dgVariables.Name = "dgVariables";
            this.dgVariables.RowHeadersVisible = false;
            this.dgVariables.RowTemplate.Height = 25;
            this.dgVariables.Size = new System.Drawing.Size(461, 426);
            this.dgVariables.TabIndex = 8;
            this.dgVariables.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Update_Click);
            // 
            // ReqID
            // 
            this.ReqID.HeaderText = "ReqID";
            this.ReqID.Name = "ReqID";
            this.ReqID.ReadOnly = true;
            this.ReqID.Visible = false;
            // 
            // SimVarName
            // 
            this.SimVarName.HeaderText = "SimVar";
            this.SimVarName.Name = "SimVarName";
            this.SimVarName.ReadOnly = true;
            this.SimVarName.Width = 250;
            // 
            // SimVarUnit
            // 
            this.SimVarUnit.HeaderText = "Unit";
            this.SimVarUnit.Name = "SimVarUnit";
            this.SimVarUnit.Visible = false;
            // 
            // SimVarValue
            // 
            this.SimVarValue.HeaderText = "Value";
            this.SimVarValue.Name = "SimVarValue";
            this.SimVarValue.ReadOnly = true;
            // 
            // Update
            // 
            this.Update.HeaderText = "Update";
            this.Update.Name = "Update";
            this.Update.Text = "Update";
            this.Update.ToolTipText = "Update";
            this.Update.UseColumnTextForButtonValue = true;
            this.Update.Width = 50;
            // 
            // SimConnectHandler_DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.dgVariables);
            this.Controls.Add(this.gpRequest);
            this.Controls.Add(this.gpConnect);
            this.Controls.Add(this.cbConnected);
            this.Name = "SimConnectHandler_DemoForm";
            this.Text = "SimConnect Test";
            ((System.ComponentModel.ISupportInitialize)(this.txtSimConnectPort)).EndInit();
            this.gpConnect.ResumeLayout(false);
            this.gpConnect.PerformLayout();
            this.gpRequest.ResumeLayout(false);
            this.gpRequest.PerformLayout();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn ReqID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVarValue;
        private System.Windows.Forms.DataGridViewButtonColumn Update;
    }
}