using AppQuaySo.Model;
using Core.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuaySo.Services
{
    public class KetQuaServices
    {
        DataAdapper dapper;
        public KetQuaServices()
        {
            dapper = new DataAdapper(Core.DataContext.DataAccess.GetConnectStr());
        }

        public async Task<DataTable> GetRanDom(object param)
        {
            {
                DataTable data = await dapper.GetDataTableAsync("sp_GetListRegisterInEmp", param, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public ResultModel SaveData(object param)
        {
            ResultModel result = new ResultModel();
            string mess = string.Empty;
            int messcode = 0;
            DataTable data = dapper.GetDataTableOut("sp_GetListRegisterInEmp", param, ref mess, ref messcode, commandType: CommandType.StoredProcedure);

            result.Message = mess;
            result.MessageCode = messcode;

            return result;
        }
    }
}
