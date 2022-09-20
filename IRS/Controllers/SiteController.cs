using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class SiteController : ApiControllerBase
    {
        private readonly ISiteService _service;

        public SiteController(ISiteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetSitesByAccount()
        {
            return Ok(await _service.GetSitesByAccount());
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUploadFile([FromForm] decimal key)
        {
            return Ok(await _service.DeleteUploadFile(key));
        }

        [HttpPost]
        public async Task<ActionResult> AddFormAsync([FromForm] SiteDto model)
        {
            return Ok(await _service.AddFormAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFormAsync([FromForm] SiteDto model)
        {
            return Ok(await _service.UpdateFormAsync(model));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] SiteDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] SiteDto model)
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

        [HttpPost]
        public async Task<ActionResult> LoadData([FromBody] DataManager request, [FromQuery] string farmGuid)
        {

            var data = await _service.LoadData(request, farmGuid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
