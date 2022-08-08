using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class ShoeGlueController : ApiControllerBase
    {
        private readonly IShoeGlueService _service;

        public ShoeGlueController(IShoeGlueService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult> GetMenuPageSetting()
        {
            return Ok(await _service.GetMenuPageSetting());
        }

        [HttpGet]
        public async Task<ActionResult> GetRecipePageSetting()
        {
            return Ok(await _service.GetRecipePageSetting());
        }

        [HttpGet]
        public async Task<ActionResult> LoadDataBySite(string siteID)
        {
            return Ok(await _service.LoadDataBySite(siteID));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ShoeGlueDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] ShoeGlueDto model)
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
        public async Task<ActionResult> LoadData([FromQuery] string shoeGuid)
        {

            var data = await _service.LoadData(shoeGuid);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
