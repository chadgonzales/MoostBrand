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
    public partial class frmLogList : Form
    {
        public frmLogList()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmLogList_Load(object sender, EventArgs e)
        {
            InitListView();
        }

        public void InitListView() {
            lvwLogs.Columns.Clear();
            ListView.ColumnHeaderCollection cols = lvwLogs.Columns;
            cols.Add("Date").Width = 95;
            cols.Add("Time").Width = 95;
            cols.Add("Employee Name").Width = 160;
            cols.Add("[Location]").Width = 95;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmLogin _frmLogin = new frmLogin();
            _frmLogin.Show();

        }
    }
}
