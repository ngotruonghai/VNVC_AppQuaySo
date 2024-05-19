using AppQuaySo.Model;
using AppQuaySo.Services;
using System.Data;

namespace AppQuaySo.From
{
    public partial class frmAppMail : Form
    {
        EmpLoyInfoServices services = new EmpLoyInfoServices();
        public frmAppMail()
        {
            InitializeComponent();
        }

        private void CheckOnClick(object sender, EventArgs e)
        {
            ServerConnect server = new ServerConnect();
            server.Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataEmp = new DataTable();
                services = new EmpLoyInfoServices();
                if (Core.DataContext.DataAccess.CheckConnect() == false)
                {
                    MessageBox.Show("Kiểm tra thông tin kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(txtSoDienThoai.Text))
                {
                    MessageBox.Show("Kiểm tra thông tin số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CheckEmpInfoModel model = new CheckEmpInfoModel
                {
                    SDT = txtSoDienThoai.Text,
                    Activity = "CheckNumber"
                };
                dataEmp = await services.GetEmployInfo(model);

                if (dataEmp == null || dataEmp.Rows.Count == 0)
                {
                    MessageBox.Show("Bạn chưa đăng ký ứng dụng, vui lòng đăng ký", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    this.Hide();
                    frmDanhSachQuaySo frmDanhSachQuaySo = new frmDanhSachQuaySo(txtSoDienThoai.Text);
                    frmDanhSachQuaySo.ShowDialog();                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void DangKyOnClick(object sender, EventArgs e)
        {
            frmDangKy frm = new frmDangKy();
            frm.ShowDialog();
        }
    }
}
