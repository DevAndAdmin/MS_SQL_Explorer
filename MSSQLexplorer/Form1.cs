using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSSQLexplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            LoadServers();
        }

        private void LoadServers()
        {
            DataTable servers = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            if (servers != null)
            {
                foreach (DataRow dr in servers.Rows)
                {
                    //comboBox1.Items.Add(string.Concat(dr["ServerName"], "\\", dr["InstanceName"]));
                    comboBox1.Invoke((MethodInvoker)(() => comboBox1.Items.Add(string.Concat(dr["ServerName"], "\\", dr["InstanceName"]))));
                }
            }
            else
                MessageBox.Show("No db found","kosiak");
        }

        private async void LoadServersAsync()
        {
            await Task.Run(() =>
            {
                 DataTable servers = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
                if (servers != null)
                {
                    foreach (DataRow dr in servers.Rows)
                    {
                        //comboBox1.Items.Add(string.Concat(dr["ServerName"], "\\", dr["InstanceName"]));
                        comboBox1.Invoke((MethodInvoker)(() => comboBox1.Items.Add(string.Concat(dr["ServerName"], "\\", dr["InstanceName"]))));
                    }
                }
                else comboBox1.Invoke((MethodInvoker)(() => comboBox1.Text = "No db found"));
            });
        }
    }
}
