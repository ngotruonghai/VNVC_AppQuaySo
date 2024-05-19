using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataContext
{
    public class ConnectSql
    {
        private string _ServerName = string.Empty;
        private string _Username = string.Empty;
        private string _Password = string.Empty;
        private string _StrConnection = string.Empty;
        private string _TableName = string.Empty;

        public ConnectSql(string ServerName, string username, string password, string TableName = "")
        {
            _ServerName = ServerName;
            _Username = username;
            _Password = password;
            _TableName = TableName;
        }


        /// <summary>
        /// Kiểm tra connect Sql
        /// </summary>
        /// <returns></returns>
        public bool CheckConnectSql()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString()))
                {
                    try
                    {
                        // Mở kết nối
                        connection.Open();


                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        public DataTable GetListDataBaseName()
        {
            string connectionString = this.GetConnectionMasterString();

            // Tạo một DataTable để lưu trữ danh sách tên cơ sở dữ liệu
            DataTable databaseTable = new DataTable();
            databaseTable.Columns.Add("DatabaseName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Mở kết nối
                    connection.Open();

                    // Truy vấn SQL để lấy danh sách tên cơ sở dữ liệu
                    string query = "SELECT name FROM sys.databases WHERE database_id > 4"; // Loại bỏ các cơ sở dữ liệu hệ thống

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Đọc từng dòng dữ liệu và thêm vào DataTable
                            while (reader.Read())
                            {
                                string dbName = reader.GetString(0);
                                databaseTable.Rows.Add(dbName);
                            }
                        }
                    }

                    return databaseTable;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void SaveConnectStr()
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string urlFile = projectDirectory+ "\\DataLog";

            if (!Directory.Exists(urlFile))
            {
                // Tạo thư mục nếu không tồn tại
                Directory.CreateDirectory(urlFile);
            }

            DataTable data = new DataTable("DataTableLog");
            data.Columns.Add("ID", typeof(int));
            data.Columns.Add("ServerName", typeof(string));
            data.Columns.Add("User", typeof(string));
            data.Columns.Add("Password", typeof(string));
            data.Columns.Add("DatabaseName", typeof(string));
            data.Columns.Add("ConnectionStr", typeof(string));

            data.Rows.Add(0,_ServerName,_Username,_Password,_TableName ,this.GetConnectionString());

            data.WriteXml(urlFile+ "\\datalog.xml");
        }

        #region Function

        /// <summary>
        /// Tạo chuỗi connecttion
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            string strConnecttion = string.Empty;
            if (string.IsNullOrEmpty(_ServerName) || string.IsNullOrEmpty(_Username) || string.IsNullOrEmpty(_Password) || string.IsNullOrEmpty(_TableName))
            {
                return "";
            }
            strConnecttion = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", _ServerName, _TableName, _Username, _Password);
            return strConnecttion;
        }

        /// <summary>
        /// Tại chuỗi connect master
        /// </summary>
        /// <returns></returns>
        public string GetConnectionMasterString()
        {
            string strConnecttion = string.Empty;
            if (string.IsNullOrEmpty(_ServerName) || string.IsNullOrEmpty(_Username) || string.IsNullOrEmpty(_Password))
            {
                return "";
            }
            strConnecttion = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", _ServerName, "master", _Username, _Password);
            return strConnecttion;
        }

        #endregion
    }
}
