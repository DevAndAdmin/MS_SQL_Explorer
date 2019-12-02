using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace MSSQLexplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //переменные
        SqlCommand sqlcmd;
        DataTable dt;
        SqlDataAdapter sqladpt;

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
                MessageBox.Show("No db found", "kosiak");
        }

        private async void LoadServersAsync()
        {
            await Task.Run(() =>
            {
                //find ms sql servers
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

        private void showdbs()
        {
            //connect to server and ask dbs
            SqlConnection con = new SqlConnection($@"Data Source={comboBox1.Text};Initial Catalog=vnipimnbd;Integrated Security=True");
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось подключиться к выбранному серверу");
            }
            if (con != null && con.State == ConnectionState.Open)
            {
                sqlcmd = new SqlCommand("select name from sys.databases", con);
                sqladpt = new SqlDataAdapter(sqlcmd.CommandText, con);
                dt = new DataTable();
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
                sqladpt.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    comboBox2.Items.Add(string.Concat(dr["name"]));
                    //comboBox2.Invoke((MethodInvoker)(() => comboBox2.Items.Add(string.Concat(dr["name"])));
                }
                con.Close();
            }
        }

        private void showtables()
        {
            //connect to server and ask dbs
            SqlConnection con = new SqlConnection($@"Data Source={comboBox1.Text};Initial Catalog={comboBox2.Text};Integrated Security=True");
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось подключиться к выбранной БД");
            }
            if (con != null && con.State == ConnectionState.Open)
            {
                sqlcmd = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", con);
                sqladpt = new SqlDataAdapter(sqlcmd.CommandText, con);
                dt = new DataTable();
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
                sqladpt.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    listBox1.Items.Add(string.Concat(dr["TABLE_NAME"]));
                     //comboBox2.Items.Add(string.Concat(dr["TABLE_NAME"]));
                     //comboBox2.Invoke((MethodInvoker)(() => comboBox2.Items.Add(string.Concat(dr["name"])));
                }
                con.Close();
            }
        }

        private void loadtable()
        {
            SqlConnection con = new SqlConnection($@"Data Source={comboBox1.Text};Initial Catalog={comboBox2.Text};Integrated Security=True");
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось подключиться к выбранной БД");
            }
            if (con != null && con.State == ConnectionState.Open)
            {
                sqlcmd = new SqlCommand($"SELECT TOP(100) * FROM {listBox1.Text}", con);
                sqladpt = new SqlDataAdapter(sqlcmd.CommandText, con);
                dt = new DataTable();
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
                sqladpt.Fill(dt);
                bindingSource1.DataSource = dt;
                dataGridView1.DataSource = bindingSource1;
                con.Close();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Find dbs on selected ms sql server
            showdbs();
        }

        private void ComboBox2_DropDown(object sender, EventArgs e)
        {
            if (comboBox1.Text != null)
            {
                //showdbs();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
         
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            showtables();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadtable();
        }
    }
}
