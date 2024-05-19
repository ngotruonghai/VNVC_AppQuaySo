using Core.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuaySo.From
{
    public partial class ServerConnect : Form
    {
        private ConnectSql connect;
        public ServerConnect()
        {
            InitializeComponent();
            if (DataAccess.CheckConnect() == true)
            {
                LoadCombobox();
                DataTable data = DataAccess.GetDataTableConnectStr();
                cbDataBaseName.SelectedItem= data.Rows[0]["DatabaseName"].ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbClickCheck(object sender, EventArgs e)
        {
            LoadCombobox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtServerName.Text) || string.IsNullOrEmpty(txtPassWord.Text) || string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(cbDataBaseName.SelectedItem.ToString()))
                {
                    MessageBox.Show("Kiểm tra thông tin kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                connect = new ConnectSql(txtServerName.Text, txtUser.Text, txtPassWord.Text, cbDataBaseName.SelectedItem.ToString());
                if (connect.CheckConnectSql() == true)
                {
                    connect.SaveConnectStr();
                    MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kiểm tra thông tin kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void LoadCombobox()
        {
            try
            {
                cbDataBaseName.Items.Clear();
                connect = new ConnectSql(txtServerName.Text, txtUser.Text, txtPassWord.Text);
                DataTable data = connect.GetListDataBaseName();
                if (data == null || data.Rows.Count < 0)
                {
                    MessageBox.Show("Kiểm tra thông tin kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        cbDataBaseName.Items.Add(data.Rows[i][0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
