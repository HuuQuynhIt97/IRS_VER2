using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class XAccountController : ApiControllerBase
    {
        private readonly IXAccountService _service;

        public XAccountController(IXAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetXAccounts()
        {
            return Ok(await _service.GetXAccounts());
        }

        [HttpGet]
        public async Task<ActionResult> GetXAccountsBySite(string siteID)
        {
            return Ok(await _service.GetXAccountsBySite(siteID));
        }



        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] XAccountDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] XAccountDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddFormAsync([FromForm] XAccountDto model)
        {
            return Ok(await _service.AddFormAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFormAsync([FromForm] XAccountDto model)
        {
            return Ok(await _service.UpdateFormAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> LockAsync(decimal id)
        {
            return StatusCodeResult(await _service.LockAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(decimal id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetByIDAsync(decimal id)
        {
            return Ok(await _service.GetByIDAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUploadFile([FromForm] decimal key)
        {
            return Ok(await _service.DeleteUploadFile(key));
        }
        [HttpPost]
        public async Task<ActionResult> UploadAvatar([FromQuery] decimal key)
        {
            return Ok(await _service.UploadAvatar(key));
        }
        [HttpPost]
        public async Task<ActionResult> ShowPassword([FromForm] decimal key)
        {
            return Ok(await _service.ShowPassword(key));
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(XChangePasswordDto changePassword)
        {
            return Ok(await _service.ChangePassword(changePassword));
        }
        [HttpPost]
        public async Task<ActionResult> StorePermission([FromBody] StorePermissionDto model)
        {
            return Ok(await _service.StorePermission(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetPermissionsDropdown(string lang, string accountGuid)
        {
            return Ok(await _service.GetPermissionsDropdown(accountGuid, lang));
        }

        [HttpGet]
        public async Task<ActionResult> GetPermissions(string lang, string accountGuid)
        {
            return Ok(await _service.GetPermissions(accountGuid, lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetProfile(string key)
        {
            return Ok(await _service.GetProfile(key));
        }
        [HttpPost]
        public async Task<ActionResult> StoreProfile([FromBody] StoreProfileDto model)
        {
            return Ok(await _service.StoreProfile(model));
        }
        [HttpPost]
        public async Task<ActionResult> LoadData([FromBody] DataManager request, string farmGuid)
        {

            var data = await _service.LoadData(request, farmGuid);
            return Ok(data);
        }


        [HttpGet]
        public async Task<ActionResult> GetRejectsByAcceptance(string farmGuid)
        {

            var data = await _service.GetRejectsByAcceptance(farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetRejectsByRequisition(string farmGuid)
        {

            var data = await _service.GetRejectsByRequisition(farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetRejectsByRepair(string farmGuid)
        {

            var data = await _service.GetRejectsByRepair(farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetRejectsBySalesOrder(string farmGuid)
        {

            var data = await _service.GetRejectsBySalesOrder(farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetRejectsByPigDisease(string farmGuid)
        {

            var data = await _service.GetRejectsByPigDisease(farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetApproveByPigDisease(string farmGuid)
        {

            var data = await _service.GetApproveByPigDisease(farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetRecordByPigDisease(string farmGuid)
        {

            var data = await _service.GetRecordByPigDisease(farmGuid);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetXAccountsForDropdown(string farmGuid)
        {

            var data = await _service.GetXAccountsForDropdown(farmGuid);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }

         [HttpPost]
        public async Task<ActionResult> GetDataDropdownlist([FromBody] DataManager request)
        {
           
            return Ok(await _service.GetDataDropdownlist(request));
        }

    }
}
