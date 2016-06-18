using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDORM.Tools
{
    public partial class frmGuid : Form
    {
        public frmGuid()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            cmbDBType.Items.Insert(0, "MSSQL");
            cmbDBType.Items.Insert(1, "MySQL");
            cmbDBType.Items.Insert(2, "Oracle");
            cmbDBType.Items.Insert(3, "SQLite");
            cmbDBType.Items.Insert(4, "SQLCE");
            cmbDBType.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain myfrmMain = new frmMain(cmbDBType.SelectedIndex);
            myfrmMain.Show();
        }
    }
}
