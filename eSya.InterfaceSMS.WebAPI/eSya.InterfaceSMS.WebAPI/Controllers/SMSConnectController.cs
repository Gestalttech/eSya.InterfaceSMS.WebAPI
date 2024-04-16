using eSya.InterfaceSMS.DO;
using eSya.InterfaceSMS.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.InterfaceSMS.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SMSConnectController : ControllerBase
    {
        private readonly ISMSConnectRepository _smsconnectRepository;

        public SMSConnectController(ISMSConnectRepository smsconnectRepository)
        {
            _smsconnectRepository = smsconnectRepository;
        }

        #region SMS Connect
        /// <summary>
        /// Getting Locations by Business ID.
        /// UI Reffered - SMS Connect Dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocationByBusinessID(int BusinessId)
        {
            var locs = await _smsconnectRepository.GetBusinessLocationByBusinessID(BusinessId);
            return Ok(locs);
        }
        /// <summary>
        /// Getting Active Entities.
        /// UI Reffered - SMS Connect Dropdown
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveEntites()
        {
            var entities = await _smsconnectRepository.GetActiveEntites();
            return Ok(entities);
        }
        /// <summary>
        /// Getting ISD Code by BusineeKey.
        /// UI Reffered - SMS Connect display ISD Code in to Label
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLocationISDCodeByBusinessKey(int BusinessKey)
        {
            var isd = await _smsconnectRepository.GetLocationISDCodeByBusinessKey(BusinessKey);
            return Ok(isd);
        }
        /// <summary>
        /// Getting SMS Connect by Business ID.
        /// UI Reffered - SMS Connect Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSMSConnectbyBusinessID(int BusinessId)
        {
            var sms = await _smsconnectRepository.GetSMSConnectbyBusinessID(BusinessId);
            return Ok(sms);
        }
        /// <summary>
        /// Insert Insert Or Update into SMS Connect .
        /// UI Reffered -SMS Connect
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateSMSConnect(DO_SMSConnect obj)
        {
            var msg = await _smsconnectRepository.InsertOrUpdateSMSConnect(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De SMS Connect.
        /// UI Reffered - SMS Connect
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveSMSConnect(DO_SMSConnect obj)
        {
            var res = await _smsconnectRepository.ActiveOrDeActiveSMSConnect(obj);
            return Ok(res);
        }
        #endregion
    }
}
