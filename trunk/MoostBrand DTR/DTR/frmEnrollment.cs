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
    public partial class frmEnrollment : Form, DPFP.Capture.EventHandler
    {
        private DPFP.Capture.Capture Capturer;
        private DPFP.Processing.Enrollment Enroller;
        private DPFP.Template Template;

        EmployeeRepo _empRepo = new EmployeeRepo();
        EmployeeRegistrationRepo _empRegRepo = new EmployeeRegistrationRepo();

        private const uint TotalSamplesNeeded = 4;

        public string EmployeeId { get; set; }

        public frmEnrollment()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            CenterPanel();

            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            this.btnCancel.Cursor = Cursors.Hand;
            this.lblCancel.Click += new System.EventHandler(this.btnClose_Click);
            
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.Cursor = Cursors.Hand;
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnSearch.Click += btnSearch_Click;
            this.btnSearch.Cursor = Cursors.Hand;
            this.lblSearch.Click += btnSearch_Click;
            this.txtSearch.KeyUp += txtSearch_KeyUp;

            this.btnUploadPic.Click += btnUploadPic_Click;
        }

        private void CenterPanel()
        {
            pnlMain.Location = new Point(
            this.ClientSize.Width / 2 - pnlMain.Size.Width / 2,
            this.ClientSize.Height / 2 - pnlMain.Size.Height / 2);
            pnlMain.Anchor = AnchorStyles.None;
        }

        private void frmEmployeeList_Load(object sender, EventArgs e)
        {
            InitListView();
            LoadList();

            //Generate fingerprint images
            for (int cnt = 2; cnt <= 4; cnt++)
            {
                PictureBox imgFP = new PictureBox();
                imgFP.Name = "imgFingerPrint" + cnt.ToString();
                imgFP.Width = imgFingerPrint1.Width;
                imgFP.Height = imgFingerPrint1.Height;
                imgFP.Visible = true;
                imgFP.BackColor = Color.White;
                imgFP.Margin = imgFingerPrint1.Margin;
                
                //new Rectangle(3, 3, 15, 3);
                layoutFingerprint.Controls.Add(imgFP);
            }

            InitializeFileDialog();

            Init();
        }

        public void LoadList(string searchString = "") {
            string search = searchString.ToLower();

            lvwEmployeeList.Items.Clear();
            EmployeeRepo _employeeRepo = new EmployeeRepo();

            List<Employee> lstEmployee = _employeeRepo.List();
            if (!String.IsNullOrEmpty(searchString)) {
                lstEmployee = lstEmployee.FindAll(p => 
                    p.EMPID.ToLower().Contains(search) ||
                    p.FirstName.ToLower().Contains(search) ||
                    p.MiddleName.ToLower().Contains(search) ||
                    p.LastName.ToLower().Contains(search));
            }

            foreach (Employee emp in lstEmployee) {
                ListViewItem list = new ListViewItem();
                list.Text = emp.EMPID;
                list.SubItems.Add(emp.name);

                lvwEmployeeList.Items.Add(list);
            }
        }

        public void InitListView()
        {
            lvwEmployeeList.Columns.Clear();
            ListView.ColumnHeaderCollection cols = lvwEmployeeList.Columns;
            cols.Add("Id").Width = 0;
            cols.Add("Name").Width = 250;
        }

        private void lvwEmployeeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwEmployeeList.SelectedItems.Count > 0)
            {
                EmployeeId = lvwEmployeeList.SelectedItems[0].Text;

                ShowEmployeeDetails(EmployeeId);
            }
        }

        private void ShowEmployeeDetails(string employeeId) {
            Employee _employee = _empRepo.GetByEmployeeId(employeeId);

            txtFName.Text = _employee.FirstName;
            txtMName.Text = _employee.MiddleName;
            txtLName.Text = _employee.LastName;
            txtSuffix.Text = _employee.Suffix;

            if (!String.IsNullOrEmpty(_employee.ImagePath))
            {
                picEmployee.ImageLocation = Path.Combine(Common.imageFolderPath, _employee.ImagePath);
            }
            else {
                picEmployee.Image = DTR.Properties.Resources.no_image;
            }

            EmployeeRegistration _empReg = _empRegRepo.GetByEmployeeId(employeeId);
            pnlScanFingerprints.Visible = true;

            if (_empReg != null) {
                if (!String.IsNullOrEmpty(_empReg.ScanTemplate))
                {
                    pnlScanFingerprints.Visible = false;
                }
            }
        }

        public void Init()
        {
            try
            {
                Capturer = CapturerSingleton.Instance;

                //Capturer = new DPFP.Capture.Capture();

                if (Capturer != null)
                    Capturer.EventHandler = this;
                else
                    MessageBox.Show("Can't initiate capture operation!");
            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Enroller = new DPFP.Processing.Enrollment();
            UpdateFingerprintSamplingStatus();

            Start();
        }

        protected void Start()
        {
            if (Capturer != null)
            {
                try { Capturer.StartCapture(); }
                catch { MessageBox.Show("Can't initiate capture!"); }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try { Capturer.StopCapture(); }
                catch { MessageBox.Show("Can't terminate capture!"); }
            }
        }

        private PictureBox GetCurrPicBoxForFingerprint()
        {
            uint currPicBoxCounter = 0;
            currPicBoxCounter = (TotalSamplesNeeded - Enroller.FeaturesNeeded) + 1;

            if (currPicBoxCounter <= TotalSamplesNeeded)
            {
                PictureBox img = (PictureBox)layoutFingerprint.Controls.Find("imgFingerPrint" + currPicBoxCounter.ToString(), true)[0];

                return img;
            }
            else
            {
                return null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string empId = Prompt.ShowDialog("Employee ID:", "Please enter the employee Id:");

            string err = ValidateSaving(empId);
            if (String.IsNullOrEmpty(err))
            {
                EmployeeRegistration _empReg = new EmployeeRegistration();
                _empReg = _empRegRepo.GetByEmployeeId(empId);
                if (_empReg == null)
                {
                    _empReg = new EmployeeRegistration();

                    _empReg.EmpId = EmployeeId;
                    _empReg.ImagePath = "";
                }

                if (Template != null) {
                    _empReg.ScanTemplate = DpfpHelper.ConvertTemplateToBase64(Template);
                }
                
                string newFileName = "";
                //If the employee chosen a file through file dialog
                if (openFileDialog1.FileName != "default")
                {
                    newFileName = Path.Combine(Common.GetAppFolder(), "pics", empId + Path.GetExtension(openFileDialog1.FileName));
                    System.IO.File.Copy(openFileDialog1.FileName, newFileName, true);

                    _empReg.ImagePath = Path.GetFileName(newFileName);
                }

                _empRegRepo.Enroll(_empReg);

                MessageBox.Show("Employee record has been enrolled");



                




                //frmLog main = new frmLog();
                //main.Show();

                Capturer = null;

                frmLog _frmLog = (frmLog) Application.OpenForms["frmLog"];
                _frmLog.Init();
                _frmLog.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show(err);
            }
        }

        private string ValidateSaving(string empId) { 
            if(String.IsNullOrEmpty(empId)){
                return "Employee ID is required";
            }
            
            Employee _employee = _empRepo.GetByEmployeeId(empId);
            if(_employee == null){
                return "Employee doesn't exists";
            }

            if (_employee.EMPID != EmployeeId)
            {
                return "Employee Id doesn't match";
            }

            return String.Empty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Capturer = null;
            //frmLog main = new frmLog();
            //main.Show();
            frmLog _frmLog = (frmLog) Application.OpenForms["frmLog"];
            _frmLog.Init();
            _frmLog.Show();

            
            this.Close();
        }

        protected void Process(DPFP.Sample Sample)
        {
            Bitmap bitmap = DpfpHelper.ConvertSampleToBitmap(Sample);

            PictureBox img = GetCurrPicBoxForFingerprint();
            img.Image = new Bitmap(bitmap, imgFingerPrint1.Size);

            DPFP.FeatureSet features = DpfpHelper.ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

            // Check quality of the sample and add to enroller if it's good
            if (features != null) try
                {
                    Enroller.AddFeatures(features);
                }
                finally
                {
                    UpdateFingerprintSamplingStatus();

                    // Check if template has been created.
                    switch (Enroller.TemplateStatus)
                    {
                        case DPFP.Processing.Enrollment.Status.Ready:
                            OnTemplate(Enroller.Template);
                            //SetPrompt("Click Close, and then click Fingerprint Verification.");
                            Stop();
                            break;

                        case DPFP.Processing.Enrollment.Status.Failed:
                            Enroller.Clear();
                            Stop();
                            UpdateFingerprintSamplingStatus();
                            OnTemplate(null);
                            Start();
                            break;
                    }
                }
        }

        private void UpdateFingerprintSamplingStatus()
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                lblFingerprintSampleCnt.Text = String.Format("Fingerprint samples needed: {0}", Enroller.FeaturesNeeded);

                PictureBox picBox = GetCurrPicBoxForFingerprint();

                if (picBox != null)
                {
                    picBox.BorderStyle = BorderStyle.FixedSingle;
                }
            }));
        }

        private void OnTemplate(DPFP.Template template)
        {
            Template = template;
            //VerifyButton.Enabled = SaveButton.Enabled = (Template != null);
            //if (Template != null)
            //    MessageBox.Show("The fingerprint template is ready for fingerprint verification.", "Fingerprint Enrollment");
            //else
            //    MessageBox.Show("The fingerprint template is not valid. Repeat fingerprint enrollment.", "Fingerprint Enrollment");
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

        private void InitializeFileDialog()
        {
            openFileDialog1.Filter = "Image Files (JPG,PNG,GIF)|*.JPG;*.PNG;*.GIF";
            openFileDialog1.DefaultExt = ".jpg|.png|.gif";
            openFileDialog1.AddExtension = true;
        }

        private void btnUploadPic_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picEmployee.ImageLocation = openFileDialog1.FileName;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadList(txtSearch.Text);
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadList(txtSearch.Text);
            }
        }
    }
}