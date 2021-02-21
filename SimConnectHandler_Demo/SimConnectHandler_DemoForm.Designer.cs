
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSimConnectPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSimConnectServer
            // 
            this.lblSimConnectServer.AutoSize = true;
            this.lblSimConnectServer.Location = new System.Drawing.Point(35, 12);
            this.lblSimConnectServer.Name = "lblSimConnectServer";
            this.lblSimConnectServer.Size = new System.Drawing.Size(84, 15);
            this.lblSimConnectServer.TabIndex = 0;
            this.lblSimConnectServer.Text = "MSFS 2020 PC:";
            // 
            // txtSimConnectServer
            // 
            this.txtSimConnectServer.Location = new System.Drawing.Point(125, 9);
            this.txtSimConnectServer.Name = "txtSimConnectServer";
            this.txtSimConnectServer.Size = new System.Drawing.Size(100, 23);
            this.txtSimConnectServer.TabIndex = 1;
            this.txtSimConnectServer.Text = "localhost";
            // 
            // lblSimConnectPort
            // 
            this.lblSimConnectPort.AutoSize = true;
            this.lblSimConnectPort.Location = new System.Drawing.Point(35, 43);
            this.lblSimConnectPort.Name = "lblSimConnectPort";
            this.lblSimConnectPort.Size = new System.Drawing.Size(32, 15);
            this.lblSimConnectPort.TabIndex = 2;
            this.lblSimConnectPort.Text = "Port:";
            // 
            // txtSimConnectPort
            // 
            this.txtSimConnectPort.Location = new System.Drawing.Point(125, 43);
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
            this.pbConnect.Location = new System.Drawing.Point(296, 9);
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
            this.cbConnected.Location = new System.Drawing.Point(518, 12);
            this.cbConnected.Name = "cbConnected";
            this.cbConnected.Size = new System.Drawing.Size(162, 19);
            this.cbConnected.TabIndex = 5;
            this.cbConnected.Text = "Connected to MSFS 2020?";
            this.cbConnected.UseVisualStyleBackColor = true;
            // 
            // SimConnectTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbConnected);
            this.Controls.Add(this.pbConnect);
            this.Controls.Add(this.txtSimConnectPort);
            this.Controls.Add(this.lblSimConnectPort);
            this.Controls.Add(this.txtSimConnectServer);
            this.Controls.Add(this.lblSimConnectServer);
            this.Name = "SimConnectTestForm";
            this.Text = "SimConnectTest";
            ((System.ComponentModel.ISupportInitialize)(this.txtSimConnectPort)).EndInit();
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
    }
}