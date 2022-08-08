using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class XAccountGroupController : ApiControllerBase
    {
        private readonly IXAccountGroupService _service;

        public XAccountGroupController(IXAccountGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] XAccountGroupDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] XAccountGroupDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
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

        [HttpGet]
        public async Task<ActionResult> GetAccountGroup()
        {
            return Ok(await _service.GetAccountGroup());
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
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
    }
}
