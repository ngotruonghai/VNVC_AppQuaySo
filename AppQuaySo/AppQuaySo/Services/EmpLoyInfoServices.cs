using AppQuaySo.Model;
using Core.DataContext;
using System.Data;

namespace AppQuaySo.Services
{
    public class EmpLoyInfoServices
    {
        DataAdapper dapper;
        public EmpLoyInfoServices()
        {
            dapper = new DataAdapper(Core.DataContext.DataAccess.GetConnectStr());
        }


        public async Task<DataTable> GetEmployInfo(object param)
        {
            DataTable data = await dapper.GetDataTableAsync("sp_GetListEmploy", param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public ResultModel SaveData(object param)
        {
            ResultModel result = new ResultModel();
            string mess = string.Empty;
            int messcode = 0;
            DataTable data = dapper.GetDataTableOut("sp_GetListEmploy", param,ref mess,ref messcode, commandType: CommandType.StoredProcedure);
           
            result.Message = mess;
            result.MessageCode = messcode;

            return result;
        }

        public async Task<DataTable> GetListOfStatistics(object param)
        {
            DataTable data = await dapper.GetDataTableAsync("sp_ListOfStatistics", param, commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}
