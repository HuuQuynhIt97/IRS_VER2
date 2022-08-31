using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class ShoeController : ApiControllerBase
    {
        private readonly IShoeService _service;

        public ShoeController(IShoeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult> LoadDataBySite(string siteID)
        {
            return Ok(await _service.LoadDataBySite(siteID));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ShoeDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] ShoeDto model)
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
