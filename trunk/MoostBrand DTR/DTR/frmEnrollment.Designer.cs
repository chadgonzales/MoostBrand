namespace DTR
{
    partial class frmEnrollment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEnrollment));
            this.lvwEmployeeList = new System.Windows.Forms.ListView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFingerprintSampleCnt = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picEmployee = new System.Windows.Forms.PictureBox();
            this.btnUploadPic = new System.Windows.Forms.Panel();
            this.lblUploadPic = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.txtLName = new System.Windows.Forms.TextBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.txtMName = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.pnlScanFingerprints = new System.Windows.Forms.Panel();
            this.layoutFingerprint = new System.Windows.Forms.FlowLayoutPanel();
            this.imgFingerPrint1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Panel();
            this.lblCancel = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Panel();
            this.lblSave = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmployee)).BeginInit();
            this.btnUploadPic.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlScanFingerprints.SuspendLayout();
            this.layoutFingerprint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgFingerPrint1)).BeginInit();
            this.panel13.SuspendLayout();
            this.btnSearch.SuspendLayout();
            this.panel14.SuspendLayout();
            this.btnCancel.SuspendLayout();
            this.btnSave.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwEmployeeList
            // 
            this.lvwEmployeeList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lvwEmployeeList.FullRowSelect = true;
            this.lvwEmployeeList.GridLines = true;
            this.lvwEmployeeList.Location = new System.Drawing.Point(18, 16);
            this.lvwEmployeeList.Name = "lvwEmployeeList";
            this.lvwEmployeeList.Size = new System.Drawing.Size(284, 442);
            this.lvwEmployeeList.TabIndex = 40;
            this.lvwEmployeeList.UseCompatibleStateImageBehavior = false;
            this.lvwEmployeeList.View = System.Windows.Forms.View.Details;
            this.lvwEmployeeList.SelectedIndexChanged += new System.EventHandler(this.lvwEmployeeList_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(12, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(177, 15);
            this.txtSearch.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(45, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 16);
            this.label2.TabIndex = 58;
            this.label2.Text = "Click employee name to select";
            // 
            // lblFingerprintSampleCnt
            // 
            this.lblFingerprintSampleCnt.AutoSize = true;
            this.lblFingerprintSampleCnt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFingerprintSampleCnt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.lblFingerprintSampleCnt.Location = new System.Drawing.Point(24, 208);
            this.lblFingerprintSampleCnt.Name = "lblFingerprintSampleCnt";
            this.lblFingerprintSampleCnt.Size = new System.Drawing.Size(190, 16);
            this.lblFingerprintSampleCnt.TabIndex = 99;
            this.lblFingerprintSampleCnt.Text = "Fingerprint samples needed:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "default";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(118)))), ((int)(((byte)(164)))));
            this.panel1.Controls.Add(this.lvwEmployeeList);
            this.panel1.Location = new System.Drawing.Point(16, 137);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 475);
            this.panel1.TabIndex = 107;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::DTR.Properties.Resources.icon_click;
            this.pictureBox1.Location = new System.Drawing.Point(16, 98);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 33);
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnUploadPic);
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Location = new System.Drawing.Point(352, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(643, 338);
            this.panel2.TabIndex = 108;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.panel3.Controls.Add(this.picEmployee);
            this.panel3.Location = new System.Drawing.Point(19, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(247, 244);
            this.panel3.TabIndex = 106;
            // 
            // picEmployee
            // 
            this.picEmployee.BackColor = System.Drawing.Color.White;
            this.picEmployee.Location = new System.Drawing.Point(14, 13);
            this.picEmployee.Name = "picEmployee";
            this.picEmployee.Size = new System.Drawing.Size(219, 217);
            this.picEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEmployee.TabIndex = 87;
            this.picEmployee.TabStop = false;
            // 
            // btnUploadPic
            // 
            this.btnUploadPic.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadPic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUploadPic.BackgroundImage")));
            this.btnUploadPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUploadPic.Controls.Add(this.lblUploadPic);
            this.btnUploadPic.Location = new System.Drawing.Point(19, 270);
            this.btnUploadPic.Name = "btnUploadPic";
            this.btnUploadPic.Size = new System.Drawing.Size(247, 52);
            this.btnUploadPic.TabIndex = 114;
            // 
            // lblUploadPic
            // 
            this.lblUploadPic.AutoSize = true;
            this.lblUploadPic.BackColor = System.Drawing.Color.Transparent;
            this.lblUploadPic.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadPic.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.lblUploadPic.Location = new System.Drawing.Point(64, 17);
            this.lblUploadPic.Name = "lblUploadPic";
            this.lblUploadPic.Size = new System.Drawing.Size(122, 18);
            this.lblUploadPic.TabIndex = 0;
            this.lblUploadPic.Text = "UPLOAD IMAGE";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 222F));
            this.tableLayoutPanel1.Controls.Add(this.panel12, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel11, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel10, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel8, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel9, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(286, 15);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(342, 250);
            this.tableLayoutPanel1.TabIndex = 107;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.White;
            this.panel12.Controls.Add(this.txtSuffix);
            this.panel12.Location = new System.Drawing.Point(123, 189);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(208, 45);
            this.panel12.TabIndex = 116;
            // 
            // txtSuffix
            // 
            this.txtSuffix.BackColor = System.Drawing.Color.White;
            this.txtSuffix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSuffix.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtSuffix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.txtSuffix.Location = new System.Drawing.Point(12, 14);
            this.txtSuffix.MaxLength = 1;
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.ReadOnly = true;
            this.txtSuffix.Size = new System.Drawing.Size(184, 19);
            this.txtSuffix.TabIndex = 102;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.White;
            this.panel11.Controls.Add(this.txtLName);
            this.panel11.Location = new System.Drawing.Point(123, 127);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(208, 45);
            this.panel11.TabIndex = 115;
            // 
            // txtLName
            // 
            this.txtLName.BackColor = System.Drawing.Color.White;
            this.txtLName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtLName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.txtLName.Location = new System.Drawing.Point(12, 14);
            this.txtLName.MaxLength = 20;
            this.txtLName.Name = "txtLName";
            this.txtLName.ReadOnly = true;
            this.txtLName.Size = new System.Drawing.Size(184, 19);
            this.txtLName.TabIndex = 96;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.White;
            this.panel10.Controls.Add(this.txtMName);
            this.panel10.Location = new System.Drawing.Point(123, 65);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(208, 45);
            this.panel10.TabIndex = 114;
            // 
            // txtMName
            // 
            this.txtMName.BackColor = System.Drawing.Color.White;
            this.txtMName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtMName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.txtMName.Location = new System.Drawing.Point(12, 14);
            this.txtMName.MaxLength = 1;
            this.txtMName.Name = "txtMName";
            this.txtMName.ReadOnly = true;
            this.txtMName.Size = new System.Drawing.Size(184, 19);
            this.txtMName.TabIndex = 98;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(114, 56);
            this.panel5.TabIndex = 109;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(23, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 101;
            this.label4.Text = "First Name:";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 65);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(114, 56);
            this.panel6.TabIndex = 110;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(9, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 18);
            this.label3.TabIndex = 100;
            this.label3.Text = "Middle Initial:";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label5);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 127);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(114, 56);
            this.panel7.TabIndex = 111;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(25, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 18);
            this.label5.TabIndex = 99;
            this.label5.Text = "Last Name:";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label7);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(3, 189);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(114, 58);
            this.panel8.TabIndex = 112;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(58, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 18);
            this.label7.TabIndex = 103;
            this.label7.Text = "Suffix:";
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.White;
            this.panel9.Controls.Add(this.txtFName);
            this.panel9.Location = new System.Drawing.Point(123, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(208, 45);
            this.panel9.TabIndex = 113;
            // 
            // txtFName
            // 
            this.txtFName.BackColor = System.Drawing.Color.White;
            this.txtFName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.txtFName.Location = new System.Drawing.Point(12, 14);
            this.txtFName.MaxLength = 20;
            this.txtFName.Name = "txtFName";
            this.txtFName.ReadOnly = true;
            this.txtFName.Size = new System.Drawing.Size(184, 19);
            this.txtFName.TabIndex = 97;
            // 
            // pnlScanFingerprints
            // 
            this.pnlScanFingerprints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnlScanFingerprints.Controls.Add(this.lblFingerprintSampleCnt);
            this.pnlScanFingerprints.Controls.Add(this.layoutFingerprint);
            this.pnlScanFingerprints.Controls.Add(this.label6);
            this.pnlScanFingerprints.Location = new System.Drawing.Point(352, 369);
            this.pnlScanFingerprints.Name = "pnlScanFingerprints";
            this.pnlScanFingerprints.Size = new System.Drawing.Size(643, 243);
            this.pnlScanFingerprints.TabIndex = 109;
            // 
            // layoutFingerprint
            // 
            this.layoutFingerprint.Controls.Add(this.imgFingerPrint1);
            this.layoutFingerprint.Location = new System.Drawing.Point(19, 49);
            this.layoutFingerprint.Name = "layoutFingerprint";
            this.layoutFingerprint.Size = new System.Drawing.Size(606, 143);
            this.layoutFingerprint.TabIndex = 99;
            // 
            // imgFingerPrint1
            // 
            this.imgFingerPrint1.BackColor = System.Drawing.Color.White;
            this.imgFingerPrint1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgFingerPrint1.Location = new System.Drawing.Point(8, 3);
            this.imgFingerPrint1.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.imgFingerPrint1.Name = "imgFingerPrint1";
            this.imgFingerPrint1.Size = new System.Drawing.Size(135, 135);
            this.imgFingerPrint1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgFingerPrint1.TabIndex = 90;
            this.imgFingerPrint1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(22, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "SCANNED FINGERPRINT";
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(118)))), ((int)(((byte)(164)))));
            this.panel13.Controls.Add(this.btnSearch);
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Location = new System.Drawing.Point(16, 14);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(317, 71);
            this.panel13.TabIndex = 110;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(118)))), ((int)(((byte)(164)))));
            this.btnSearch.BackgroundImage = global::DTR.Properties.Resources.btn_search;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Controls.Add(this.lblSearch);
            this.btnSearch.Location = new System.Drawing.Point(226, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(76, 44);
            this.btnSearch.TabIndex = 113;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.White;
            this.lblSearch.Location = new System.Drawing.Point(10, 15);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(54, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "SEARCH";
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.White;
            this.panel14.Controls.Add(this.txtSearch);
            this.panel14.Location = new System.Drawing.Point(16, 13);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(201, 46);
            this.panel14.TabIndex = 52;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::DTR.Properties.Resources.btn_cancel;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Controls.Add(this.lblCancel);
            this.btnCancel.Location = new System.Drawing.Point(878, 630);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 49);
            this.btnCancel.TabIndex = 112;
            // 
            // lblCancel
            // 
            this.lblCancel.AutoSize = true;
            this.lblCancel.BackColor = System.Drawing.Color.Transparent;
            this.lblCancel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancel.ForeColor = System.Drawing.Color.White;
            this.lblCancel.Location = new System.Drawing.Point(27, 15);
            this.lblCancel.Name = "lblCancel";
            this.lblCancel.Size = new System.Drawing.Size(69, 18);
            this.lblCancel.TabIndex = 0;
            this.lblCancel.Text = "CANCEL";
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::DTR.Properties.Resources.btn_login;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Controls.Add(this.lblSave);
            this.btnSave.Location = new System.Drawing.Point(746, 630);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 49);
            this.btnSave.TabIndex = 111;
            // 
            // lblSave
            // 
            this.lblSave.AutoSize = true;
            this.lblSave.BackColor = System.Drawing.Color.Transparent;
            this.lblSave.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.lblSave.Location = new System.Drawing.Point(35, 15);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(45, 18);
            this.lblSave.TabIndex = 0;
            this.lblSave.Text = "SAVE";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.panel13);
            this.pnlMain.Controls.Add(this.pictureBox1);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.btnCancel);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.btnSave);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.pnlScanFingerprints);
            this.pnlMain.Location = new System.Drawing.Point(45, 28);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1012, 691);
            this.pnlMain.TabIndex = 113;
            // 
            // frmEnrollment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(136)))), ((int)(((byte)(192)))));
            this.BackgroundImage = global::DTR.Properties.Resources.bg_blue;
            this.ClientSize = new System.Drawing.Size(1090, 742);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEnrollment";
            this.Text = "Employee Enrollment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmEmployeeList_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEmployee)).EndInit();
            this.btnUploadPic.ResumeLayout(false);
            this.btnUploadPic.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.pnlScanFingerprints.ResumeLayout(false);
            this.pnlScanFingerprints.PerformLayout();
            this.layoutFingerprint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgFingerPrint1)).EndInit();
            this.panel13.ResumeLayout(false);
            this.btnSearch.ResumeLayout(false);
            this.btnSearch.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.btnCancel.ResumeLayout(false);
            this.btnCancel.PerformLayout();
            this.btnSave.ResumeLayout(false);
            this.btnSave.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwEmployeeList;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFingerprintSampleCnt;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLName;
        private System.Windows.Forms.PictureBox picEmployee;
        private System.Windows.Forms.Panel pnlScanFingerprints;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel layoutFingerprint;
        private System.Windows.Forms.PictureBox imgFingerPrint1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel btnCancel;
        private System.Windows.Forms.Label lblCancel;
        private System.Windows.Forms.Panel btnSave;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel btnSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel btnUploadPic;
        private System.Windows.Forms.Label lblUploadPic;
        private System.Windows.Forms.Panel pnlMain;
    }
}