using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Core.DataContext
{
    public class DataAdapper
    {
        private string _ConnectStr = string.Empty;
        public DataAdapper(string ConnectStr)
        {
            _ConnectStr = ConnectStr;
        }

        public async Task<DataTable> GetDataTableAsync(string sqlScript, object parameter = null, CommandType commandType = CommandType.Text, int Timeout = 20)
        {
            DataTable data = new DataTable();
            using (var _context = new SqlConnection(_ConnectStr))
            {
                using var reader = await _context.ExecuteReaderAsync(sqlScript, parameter, commandType: commandType, commandTimeout: Timeout);
                data.Load(reader);
            }
            return data;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlScript, object parameter = null, CommandType commandType = CommandType.Text, int Timeout = 20)
        {
            using var _context = new SqlConnection(_ConnectStr);
            var data = await _context.QueryAsync<T>(sqlScript, parameter, commandType: commandType, commandTimeout: Timeout);
            return data;
        }

        public int Execute(string sqlScript, object parameter = null, CommandType commandType = CommandType.Text, int Timeout = 20)
        {
            using var _context = new SqlConnection(_ConnectStr);
            _context.Open();
            using var transaction = _context.BeginTransaction();
            try
            {
                var result = _context.Execute(sqlScript, parameter, transaction: transaction, commandType: commandType, commandTimeout: Timeout);
                transaction.Commit();
                _context.Close();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                _context.Close();
                throw;
            }
        }

        public DataTable GetDataTableOut(string sqlScript, object parameter, ref string Message, ref int MessageCode, CommandType commandType = CommandType.StoredProcedure, int Timeout = 20)
        {
            DataTable data = new DataTable();
            using (var _context = new SqlConnection(_ConnectStr))
            {
                try
                {
                    _context.Open();

                    // Thực thi stored procedure và lấy dữ liệu
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = sqlScript;
                        command.CommandType = commandType;
                        command.CommandTimeout = Timeout;

                        // Thêm tham số đầu vào nếu có
                        if (parameter != null)
                            command.Parameters.AddRange(GetParameters(parameter));

                        // Thêm tham số OUTPUT cho Message
                        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                        messageParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParameter);

                        var messageCode = new SqlParameter("@MessageCode", SqlDbType.Int);
                        messageCode.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageCode);

                        // Thực thi truy vấn
                        using (var reader = command.ExecuteReader())
                        {
                            data.Load(reader);
                        }

                        // Lấy giá trị OUTPUT từ tham số Message
                        if (messageParameter.Value != DBNull.Value)
                        {
                            Message = messageParameter.Value.ToString();
                            MessageCode = int.Parse(messageCode.Value.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý exception
                    Message = ex.Message;
                    MessageCode = -1; // hoặc một mã lỗi khác bạn muốn gán
                }
            }
            return data;
        }

        private SqlParameter[] GetParameters(object parameter)
        {
            var properties = parameter.GetType().GetProperties();
            var sqlParameters = new List<SqlParameter>();
            foreach (var property in properties)
            {
                sqlParameters.Add(new SqlParameter("@" + property.Name, property.GetValue(parameter)));
            }
            return sqlParameters.ToArray();
        }
    }
}
