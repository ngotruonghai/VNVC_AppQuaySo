using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataContext
{
    public static class DataAccess
    {
        public static DataTable GetDataTableConnectStr()
        {
            try
            {
                // Lấy đường dẫn thư mục hiện tại (thường là thư mục của dự án)
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string urlFile = projectDirectory + "\\DataLog";

                DataTable data = new DataTable("DataTableLog");
                data.Columns.Add("ID", typeof(int));
                data.Columns.Add("ServerName", typeof(string));
                data.Columns.Add("User", typeof(string));
                data.Columns.Add("Password", typeof(string));
                data.Columns.Add("DatabaseName", typeof(string));
                data.Columns.Add("ConnectionStr", typeof(string));

                data.ReadXml(urlFile + "\\datalog.xml");

                return data;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Kiểm tra connect sql
        /// </summary>
        /// <param name="ConnectStr"></param>
        /// <returns></returns>
        public static bool CheckConnect()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectStr()))
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

        /// <summary>
        /// Kiểm tra connect
        /// </summary>
        /// <returns></returns>
        public static string GetConnectStr()
        {
            try
            {
                DataTable data = Core.DataContext.DataAccess.GetDataTableConnectStr();
                return data.Rows[0]["ConnectionStr"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
