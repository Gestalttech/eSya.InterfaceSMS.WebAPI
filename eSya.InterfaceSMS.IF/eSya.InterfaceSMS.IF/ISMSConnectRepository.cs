using eSya.InterfaceSMS.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.InterfaceSMS.IF
{
    public interface ISMSConnectRepository
    {
        #region SMS Connect
        Task<List<DO_BusinessEntity>> GetActiveEntites();
        Task<List<DO_BusinessLocation>> GetBusinessLocationByBusinessID(int BusinessId);
        Task<DO_CountryCodes> GetLocationISDCodeByBusinessKey(int BusinessKey);
        Task<List<DO_SMSConnect>> GetSMSConnectbyBusinessID(int BusinessId);
        Task<DO_ReturnParameter> InsertOrUpdateSMSConnect(DO_SMSConnect obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveSMSConnect(DO_SMSConnect obj);
        #endregion
    }
}
