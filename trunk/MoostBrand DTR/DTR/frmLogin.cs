using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTR
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.Cursor = Cursors.Hand;
            this.lblLogin.Click += new System.EventHandler(this.btnOk_Click);

            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.Cursor = Cursors.Hand;
            this.lblCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.StartPosition = FormStartPosition.CenterScreen;

            this.txtPassword.KeyUp += txtPassword_KeyUp;
            this.txtUsername.KeyUp += txtUsername_KeyUp;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            LogIn();
        }

        private void LogIn() {
            UserRepo _userRepo = new UserRepo();

            if (_userRepo.AuthenticateAdmin(txtUsername.Text, txtPassword.Text))
            {
                Application.OpenForms["frmLog"].Hide();

                frmEnrollment frm = new frmEnrollment();
                frm.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username and/or password", 
                    "Access denied",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LogIn();
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LogIn();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}