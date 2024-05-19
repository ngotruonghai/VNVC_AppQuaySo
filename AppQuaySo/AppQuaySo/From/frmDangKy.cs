using AppQuaySo.Model;
using AppQuaySo.Services;
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
    public partial class frmDangKy : Form
    {
        EmpLoyInfoServices _services = new EmpLoyInfoServices();
        public frmDangKy()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHoVaTen.Text) || string.IsNullOrEmpty(txtSDT.Text))
            {
                MessageBox.Show("Không được để trống thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            EmpRegisterModel model = new EmpRegisterModel
            {
                Activity = "SaveData",
                HoTen = txtHoVaTen.Text,
                SDT = txtSDT.Text,
                NgaySinh = dateNgaySinh.Text,

            };
            ResultModel result = _services.SaveData(model);
            if(result.MessageCode == 0)
            {
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }
    }
}
