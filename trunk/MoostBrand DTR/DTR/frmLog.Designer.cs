namespace DTR
{
    partial class frmLog
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
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "asdff",
            "asdf",
            "asdf"}, -1, System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(134)))), ((int)(((byte)(134))))), System.Drawing.Color.Empty, new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "asdfgsg",
            "1234",
            ""}, -1);
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnEnrollEmployee = new System.Windows.Forms.Panel();
            this.lblEnrollEmployee = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnLogType = new JCS.ToggleSwitch();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lvwRecentLogs = new System.Windows.Forms.ListView();
            this.EmpName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LogTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LogType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblEmployeeName = new System.Windows.Forms.Label();
            this.picEmployee = new System.Windows.Forms.PictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.btnEnrollEmployee.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmployee)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(442, 28);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(228, 19);
            this.lblDate.TabIndex = 6;
            this.lblDate.Text = "THURSDAY MARCH 31, 2016";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(689, 18);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(185, 37);
            this.lblTime.TabIndex = 90;
            this.lblTime.Text = "8:24:00 AM";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnEnrollEmployee);
            this.pnlHeader.Controls.Add(this.lblDate);
            this.pnlHeader.Controls.Add(this.lblTime);
            this.pnlHeader.Location = new System.Drawing.Point(32, 12);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(900, 70);
            this.pnlHeader.TabIndex = 101;
            // 
            // btnEnrollEmployee
            // 
            this.btnEnrollEmployee.BackgroundImage = global::DTR.Properties.Resources.btnEnrollEmployee1;
            this.btnEnrollEmployee.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnrollEmployee.Controls.Add(this.lblEnrollEmployee);
            this.btnEnrollEmployee.Location = new System.Drawing.Point(21, 7);
            this.btnEnrollEmployee.Name = "btnEnrollEmployee";
            this.btnEnrollEmployee.Size = new System.Drawing.Size(239, 56);
            this.btnEnrollEmployee.TabIndex = 91;
            // 
            // lblEnrollEmployee
            // 
            this.lblEnrollEmployee.AutoSize = true;
            this.lblEnrollEmployee.BackColor = System.Drawing.Color.Transparent;
            this.lblEnrollEmployee.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnrollEmployee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.lblEnrollEmployee.Location = new System.Drawing.Point(43, 20);
            this.lblEnrollEmployee.Name = "lblEnrollEmployee";
            this.lblEnrollEmployee.Size = new System.Drawing.Size(158, 18);
            this.lblEnrollEmployee.TabIndex = 0;
            this.lblEnrollEmployee.Text = "ENROLL EMPLOYEE";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.label1);
            this.pnlFooter.Location = new System.Drawing.Point(368, 28);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(299, 41);
            this.pnlFooter.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Copyright 2016 Jentec - DTR Management\r\n";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.btnLogType);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Location = new System.Drawing.Point(32, 115);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(900, 483);
            this.pnlMain.TabIndex = 99;
            // 
            // btnLogType
            // 
            this.btnLogType.Checked = true;
            this.btnLogType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogType.Location = new System.Drawing.Point(99, 3);
            this.btnLogType.Name = "btnLogType";
            this.btnLogType.OffFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogType.OffForeColor = System.Drawing.Color.White;
            this.btnLogType.OffText = "LOG OUT";
            this.btnLogType.OnFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLogType.OnForeColor = System.Drawing.Color.White;
            this.btnLogType.OnText = "LOG IN";
            this.btnLogType.Size = new System.Drawing.Size(201, 48);
            this.btnLogType.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Modern;
            this.btnLogType.TabIndex = 103;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(220)))));
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.lvwRecentLogs);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(480, 57);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 402);
            this.panel4.TabIndex = 100;
            // 
            // panel8
            // 
            this.panel8.Location = new System.Drawing.Point(304, 56);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(3, 343);
            this.panel8.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.label4.Location = new System.Drawing.Point(318, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "TYPE";
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(187, 56);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(3, 343);
            this.panel5.TabIndex = 3;
            // 
            // lvwRecentLogs
            // 
            this.lvwRecentLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwRecentLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.EmpName,
            this.LogTime,
            this.LogType});
            this.lvwRecentLogs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwRecentLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(134)))), ((int)(((byte)(134)))));
            this.lvwRecentLogs.FullRowSelect = true;
            this.lvwRecentLogs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwRecentLogs.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5,
            listViewItem6});
            this.lvwRecentLogs.Location = new System.Drawing.Point(5, 55);
            this.lvwRecentLogs.Name = "lvwRecentLogs";
            this.lvwRecentLogs.Size = new System.Drawing.Size(384, 342);
            this.lvwRecentLogs.TabIndex = 2;
            this.lvwRecentLogs.UseCompatibleStateImageBehavior = false;
            this.lvwRecentLogs.View = System.Windows.Forms.View.Details;
            // 
            // EmpName
            // 
            this.EmpName.Width = 200;
            // 
            // LogTime
            // 
            this.LogTime.Width = 112;
            // 
            // LogType
            // 
            this.LogType.Width = 55;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.label3.Location = new System.Drawing.Point(190, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "LOG TIME";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.label2.Location = new System.Drawing.Point(50, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "NAME";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.lblEmployeeName);
            this.panel2.Controls.Add(this.picEmployee);
            this.panel2.Location = new System.Drawing.Point(21, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(353, 402);
            this.panel2.TabIndex = 96;
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblEmployeeName.ForeColor = System.Drawing.Color.White;
            this.lblEmployeeName.Location = new System.Drawing.Point(12, 354);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Size = new System.Drawing.Size(325, 27);
            this.lblEmployeeName.TabIndex = 90;
            this.lblEmployeeName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picEmployee
            // 
            this.picEmployee.BackColor = System.Drawing.Color.White;
            this.picEmployee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picEmployee.Location = new System.Drawing.Point(17, 15);
            this.picEmployee.Name = "picEmployee";
            this.picEmployee.Size = new System.Drawing.Size(320, 320);
            this.picEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEmployee.TabIndex = 88;
            this.picEmployee.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel7.Controls.Add(this.panel1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1156, 100);
            this.panel7.TabIndex = 100;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(136)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.pnlHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1156, 93);
            this.panel1.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel6.Controls.Add(this.panel3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 609);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1156, 85);
            this.panel6.TabIndex = 101;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.pnlFooter);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, -5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1156, 90);
            this.panel3.TabIndex = 0;
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DTR.Properties.Resources.bg_index;
            this.ClientSize = new System.Drawing.Size(1156, 694);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.panel6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmLog";
            this.Text = "JENTEC - DTR MANAGEMENT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmLog_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.btnEnrollEmployee.ResumeLayout(false);
            this.btnEnrollEmployee.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEmployee)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picEmployee;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel btnEnrollEmployee;
        private System.Windows.Forms.Label lblEnrollEmployee;
        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvwRecentLogs;
        private System.Windows.Forms.ColumnHeader EmpName;
        private System.Windows.Forms.ColumnHeader LogTime;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ColumnHeader LogType;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label4;
        private JCS.ToggleSwitch btnLogType;
    }
}