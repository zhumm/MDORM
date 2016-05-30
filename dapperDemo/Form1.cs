using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Model;
using System.Diagnostics;
using System.Data.SqlClient;
using BLL;
using DBUtility;

namespace dapperDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int totalCount = 0;
            IList<GroupPower_AtdPersonalScheduling> result = DBHelper.ExecuteSql<GroupPower_AtdPersonalScheduling>("select * from GroupPower_AtdPersonalScheduling", null, out totalCount);
            sw.Stop();
            dgvMain.DataSource = result;
            MessageBox.Show(string.Format("共获取{0}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds));
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //IList<GroupPower_AtdPersonalScheduling> result = new Respository.Respoistory<GroupPower_AtdPersonalScheduling>().Get(null);
            IList<GroupPower_AtdPersonalScheduling> result  = GroupPower_AtdPersonalSchedulingRepository.Value.GetAll();
            sw.Stop();
            //MessageBox.Show(string.Format("耗时：{0}毫秒", sw.ElapsedMilliseconds));
            //MessageBox.Show(t.SchedulDate.ToString());
            //dgvMain.DataSource = result;
            MessageBox.Show(string.Format("共获取{0}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds));
            //MessageBox.Show(totalCount.ToString());
        }
    }
}
