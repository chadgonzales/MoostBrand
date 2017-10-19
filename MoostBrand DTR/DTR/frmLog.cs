using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTR
{
    public partial class frmLog : Form, DPFP.Capture.EventHandler
    {
        LogRepo _logRepo = new LogRepo();
        EmployeeRegistrationRepo empRegRepo = new EmployeeRegistrationRepo();
        EmployeeRepo empRepo = new EmployeeRepo();

        private DPFP.Capture.Capture Capturer;
        private DPFP.Verification.Verification Verificator;

        public frmLog()
        {
            InitializeComponent();

            this.btnEnrollEmployee.Click += new System.EventHandler(this.btnEnrollEmployee_Click);
            this.lblEnrollEmployee.Click += new System.EventHandler(this.btnEnrollEmployee_Click);

            this.btnEnrollEmployee.Cursor = Cursors.Hand;
            this.lblEnrollEmployee.Cursor = Cursors.Hand;

            this.StartPosition = FormStartPosition.CenterScreen;

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //TODO
            }
        }

        private void frmLog_Load(object sender, EventArgs e)
        {
            //_logRepo.ListUnsynched();
            CenterPanel();

            DisplayCurrDateTime();

            timer1.Enabled = true;
            timer1.Interval = 1000;

            Init();

            LoadList();

            //List<Log> lstLog = _logRepo.ListRecent();
            //for (int cnt = 1; cnt <= lstLog.Count; cnt++) {
            //    tblRecentLogs.RowCount = tblRecentLogs.RowCount + 1;
            //    tblRecentLogs.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            //    Employee emp = empRepo.GetByEmployeeId(lstLog[cnt - 1].EmpId);

            //    Color textColor = Color.FromArgb(132, 132, 132);

            //    Color white = Color.FromArgb(255, 255, 255);
            //    Color gray = Color.FromArgb(249, 249, 249);

            //    Color _backColor = cnt % 2 != 0 ? white : gray;

            //    Panel pnl = new Panel() { Dock = DockStyle.Fill, BackColor = _backColor };
            //    pnl.Controls.Add(new Label() { Text = emp.name, ForeColor = textColor, Font = new Font("Arial", 11, FontStyle.Regular), Location = new Point(22, 15) });

            //    Panel pnl2 = new Panel() { Dock = DockStyle.Fill, BackColor = _backColor };
            //    pnl2.Controls.Add(new Label() { Text = lstLog[cnt - 1].ScanDate.ToString("h:mm:ss tt"), ForeColor = textColor, Font = new Font("Arial", 11, FontStyle.Regular), Location= new Point(22, 15) });

            //    tblRecentLogs.Controls.Add(pnl, 0, tblRecentLogs.RowCount - 1);
            //    tblRecentLogs.Controls.Add(pnl2, 1, tblRecentLogs.RowCount - 1);
            //}
        }

        public void LoadList(string searchString = "")
        {
            string search = searchString.ToLower();

            lvwRecentLogs.Items.Clear();

            List<Log> lstLog = _logRepo.ListRecent();

            int cnt = 0;
            foreach (Log log in lstLog)
            {
                ListViewItem list = new ListViewItem();

                Employee emp = empRepo.GetByEmployeeId(log.EmpId);
                
                list.Text = emp.name;
                list.SubItems.Add(log.ScanDate.ToString("h:mm:ss tt"));
                list.SubItems.Add(log.LogType ? "in" : "out");
                list.BackColor = cnt % 2 != 0 ? Color.FromArgb(249,249,249) : Color.White;

                lvwRecentLogs.Items.Add(list);

                cnt++;
            }
        }

        private void CenterPanel() {
            pnlMain.Location = new Point(
            this.ClientSize.Width / 2 - pnlMain.Size.Width / 2,
            this.ClientSize.Height / 2 - pnlMain.Size.Height / 2);
            pnlMain.Anchor = AnchorStyles.None;

            pnlHeader.Location = new Point(
            this.ClientSize.Width / 2 - pnlHeader.Size.Width / 2,
            pnlHeader.Location.Y);
            pnlHeader.Anchor = AnchorStyles.None;

            pnlFooter.Location = new Point(
            this.ClientSize.Width / 2 - pnlFooter.Size.Width / 2,
            pnlFooter.Location.Y);
            pnlFooter.Anchor = AnchorStyles.None;
        }

        private void btnEnrollEmployee_Click(object sender, EventArgs e)
        {
            frmLogin _frmLogin = new frmLogin();
            //_frmLogin.Show();
            _frmLogin.ShowDialog(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DisplayCurrDateTime();
        }

        private void DisplayCurrDateTime() {
            lblDate.Text = DateTime.Now.ToLongDateString().ToUpper();
            lblTime.Text = DateTime.Now.ToString("h:mm:ss tt");
        }

        public void Init()
        {
            try
            {
                Capturer = CapturerSingleton.Instance;

                //Capturer = new DPFP.Capture.Capture();

                if (Capturer != null)
                {
                    Capturer.EventHandler = this;
                }
                else {
                    MessageBox.Show("Can't initiate capture operation!");
                }
            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Verificator = new DPFP.Verification.Verification();

            Start();
        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.EventHandler = this;
                    Capturer.StartCapture();
                }
                catch
                {
                    MessageBox.Show("Can't initiate capture!");
                }
            }
        }

        private void Process(DPFP.Sample Sample)
        {
            DPFP.FeatureSet features = DpfpHelper.ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            if (features != null)
            {
                try
                {
                    List<EmployeeRegistration> lstEmpReg = empRegRepo.List();

                    bool isVerified = false;
                    bool isLogTypeValid = false;

                    foreach (EmployeeRegistration empReg in lstEmpReg)
                    {
                        if (!String.IsNullOrEmpty(empReg.ScanTemplate))
                        {
                            byte[] template = Convert.FromBase64String(empReg.ScanTemplate);
                            DPFP.Template Template = new DPFP.Template();
                            Template.DeSerialize(template);

                            DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                            Verificator.Verify(features, Template, ref result);
                            if (result.Verified)
                            {
                                isVerified = true;

                                Employee emp = empRepo.GetByEmployeeId(empReg.EmpId);
                                Log _log = _logRepo.GetLastLogByEmployeeId(empReg.EmpId);

                                if (_log != null)
                                {
                                    isLogTypeValid = _log.LogType != btnLogType.Checked;
                                }
                                else
                                {
                                    isLogTypeValid = true;
                                }

                                if (isLogTypeValid)
                                {
                                    this.Invoke(new MethodInvoker(delegate()
                                    {
                                        lblEmployeeName.Text = emp.name;

                                        if (!String.IsNullOrEmpty(emp.ImagePath))
                                        {
                                            picEmployee.ImageLocation = Path.Combine(Common.imageFolderPath, emp.ImagePath);
                                        }
                                        else
                                        {
                                            picEmployee.Image = DTR.Properties.Resources.no_image;
                                        }

                                        bool logType = btnLogType.Checked ? true : false;
                                        empRepo.Log(emp.EMPID, logType);

                                        LoadList();
                                    }));
                                }

                                break;
                            }
                        }
                    }

                    if (!isLogTypeValid)
                    {
                        if (isVerified)
                        {
                            MessageBox.Show("Invalid log type.");
                        }
                        else
                        {
                            MessageBox.Show("The fingerprint was NOT VERIFIED.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    EmailSenderClient emailSenderClient = new EmailSenderClient();
                    emailSenderClient.SendErrorNotification(ex.ToString());

                    MessageBox.Show("Error Details: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEnrollEmployee_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
