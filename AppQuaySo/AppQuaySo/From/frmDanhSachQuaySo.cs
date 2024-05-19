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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AppQuaySo.From
{
    public partial class frmDanhSachQuaySo : Form
    {
        private EmpLoyInfoServices _services;
        private KetQuaServices _KetQUaServices;
        private string _SDT = string.Empty;

        public frmDanhSachQuaySo(string SDT)
        {
            InitializeComponent();
            _SDT = SDT;

            this.GetInitialize(SDT);
            this.LoadColumListView();
            this.LoadInfo();
            this.LoadTab2();
        }

        #region  Initialize
        private async void GetInitialize(string SDT)
        {
            CheckEmpInfoModel model = new CheckEmpInfoModel
            {
                SDT = SDT,
                Activity = "GetUserInfo"
            };
            _services = new EmpLoyInfoServices();
            DataTable data = await _services.GetEmployInfo(model);
            if (data != null)
            {
                lbHoVaTen.Text = data.Rows[0]["HoVaTen"].ToString();
                lbNgayThangNamSinh.Text = data.Rows[0]["NgaySinh"].ToString();
                lbSDT.Text = data.Rows[0]["SDT"].ToString();
                lbTuoi.Text = data.Rows[0]["Tuoi"].ToString();
                txt_SoMayMan.Text = data.Rows[0]["SoDangKy"].ToString();
                lb_khunggio.Text = data.Rows[0]["DenGioDangKy"].ToString();
                lbTuGioDenGio.Text = data.Rows[0]["StartDate"].ToString();
                lb_khunggiodangky.Text = data.Rows[0]["GioThucTe"].ToString();
            }
        }
        #endregion

        private void LoadColumListView()
        {
            lsv_Danhsach.Columns.Add("STT", 50);
            lsv_Danhsach.Columns.Add("Họ tên", 200);
            lsv_Danhsach.Columns.Add("SDT", 100);
            lsv_Danhsach.Columns.Add("Số đăng ký", 100);
            lsv_Danhsach.Columns.Add("Giờ đăng ký", 100);
            lsv_Danhsach.Columns.Add("Kết quả xổ số", 150);
            lsv_Danhsach.Columns.Add("Giờ sổ số", 100);
            lsv_Danhsach.Columns.Add("Kết quả", 100);

            lsv_Danhsach.View = View.Details;
            lsv_Danhsach.FullRowSelect = true;
            lsv_Danhsach.GridLines = true;

            lsv_Monitor.Columns.Add("STT", 50);
            lsv_Monitor.Columns.Add("Số lượng vào app", 150);
            lsv_Monitor.Columns.Add("Tổng người dùng", 150);
            lsv_Monitor.Columns.Add("Tổng lượt mua số", 150);
            lsv_Monitor.Columns.Add("Tổng lượt trúng", 150);
            lsv_Monitor.Columns.Add("Tổng lượt không trúng", 150);
            lsv_Monitor.Columns.Add("Tổng lượt chưa dò", 150);

            lsv_Monitor.View = View.Details;
            lsv_Monitor.FullRowSelect = true;
            lsv_Monitor.GridLines = true;
        }

        private async void LoadInfo()
        {
            _KetQUaServices = new KetQuaServices();
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd/MM/yyyy HH:mm");
            //lbTuGioDenGio.Text = formattedDate;

            RandomNumBerModel randomNumBerModel = new RandomNumBerModel
            {
                Activity = "RanDomNumber"
            };
            DataTable data = await _KetQUaServices.GetRanDom(randomNumBerModel);
            if (data.Rows.Count > 0)
            {
                lb_randomso.Text = data.Rows[0]["KetQuaXoSo"].ToString();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            LoadInfo();
            lsv_Danhsach.Items.Clear();
            LoadCheck();
            CheckEmpInfoModel model = new CheckEmpInfoModel
            {
                Activity = "GetList",
                SDT = _SDT

            };
            DataTable data = await _KetQUaServices.GetRanDom(model);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ListViewItem lsv = new ListViewItem((i + 1).ToString());
                lsv.SubItems.Add(data.Rows[i]["HoVaTen"].ToString());
                lsv.SubItems.Add(data.Rows[i]["SDT"].ToString());
                lsv.SubItems.Add(data.Rows[i]["SoDangKy"].ToString());
                lsv.SubItems.Add(data.Rows[i]["GioThucTe"].ToString());
                lsv.SubItems.Add(data.Rows[i]["KetQuaXoSo"].ToString());
                lsv.SubItems.Add(data.Rows[i]["GioXoSo"].ToString());
                lsv.SubItems.Add(data.Rows[i]["TrungThuong"].ToString());
                lsv_Danhsach.Items.Add(lsv);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _KetQUaServices = new KetQuaServices();
            //Kiểm tra thông tin nhập
            if (string.IsNullOrEmpty(txt_SoMayMan.Text))
            {
                MessageBox.Show("Vui lòng nhập số may mắn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save
            EmpLoyRegisterModel empLoyRegister = new EmpLoyRegisterModel
            {
                Activity = "Register",
                SoDangKy = int.Parse(txt_SoMayMan.Text),
                SDT = _SDT
            };
            ResultModel result = _KetQUaServices.SaveData(empLoyRegister);
            if (result.MessageCode == 0)
            {
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK);
            }
            GetInitialize(_SDT);
        }

        private void CheckKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Đóng tất cả các form đang mở
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                form.Close();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            LoadInfo();
            lsv_Danhsach.Items.Clear();
            LoadCheck();
            CheckEmpInfoModel model = new CheckEmpInfoModel
            {
                Activity = "GetList"

            };
            DataTable data = await _KetQUaServices.GetRanDom(model);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ListViewItem lsv = new ListViewItem((i + 1).ToString());
                lsv.SubItems.Add(data.Rows[i]["HoVaTen"].ToString());
                lsv.SubItems.Add(data.Rows[i]["SDT"].ToString());
                lsv.SubItems.Add(data.Rows[i]["SoDangKy"].ToString());
                lsv.SubItems.Add(data.Rows[i]["GioThucTe"].ToString());
                lsv.SubItems.Add(data.Rows[i]["KetQuaXoSo"].ToString());
                lsv.SubItems.Add(data.Rows[i]["GioXoSo"].ToString());
                lsv.SubItems.Add(data.Rows[i]["TrungThuong"].ToString());
                lsv_Danhsach.Items.Add(lsv);
            }
        }

        #region
        private async void LoadCheck()
        {
            CheckEmpInfoModel model = new CheckEmpInfoModel
            {
                Activity = "CheckRegister",
                SDT = _SDT

            };
            await _KetQUaServices.GetRanDom(model);
        }

        private void LoadTab2()
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            date_TuNgay.Value = firstDayOfMonth;
            date_DenNgay.Value = lastDayOfMonth;
        }

        #endregion

        private async void button5_Click(object sender, EventArgs e)
        {
            ListOfStatisticsModel listOfStatisticsModel = new ListOfStatisticsModel
            {
                TuNgay = date_TuNgay.Text,
                DenNgay = date_DenNgay.Text
            };
            DataTable data = await _services.GetListOfStatistics(listOfStatisticsModel);
            lsv_Monitor.Items.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ListViewItem lsv = new ListViewItem((i + 1).ToString());
                lsv.SubItems.Add(data.Rows[i]["SoLuongVaoApp"].ToString());
                lsv.SubItems.Add(data.Rows[i]["NguoiDungApp"].ToString());
                lsv.SubItems.Add(data.Rows[i]["TongLuotQuaySo"].ToString());
                lsv.SubItems.Add(data.Rows[i]["TongLuotTrung"].ToString());
                lsv.SubItems.Add(data.Rows[i]["TongKhongLuotTrung"].ToString());
                lsv.SubItems.Add(data.Rows[i]["TongLuotChuaDo"].ToString());
                lsv_Monitor.Items.Add(lsv);
            }
        }
    }
}
