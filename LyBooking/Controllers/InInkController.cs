using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class InInkController : ApiControllerBase
    {
        private readonly IInInkService _service;

        public InInkController(IInInkService service)
        {
            _service = service;
        }


        [HttpGet("{qrCode}/{building}/{userid}")]
        public async Task<IActionResult> ScanQRCode(string qrCode)
        {
            return Ok(await _service.ScanQRCode(qrCode));
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
        public async Task<ActionResult> AddAsync([FromBody] InInkDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] InInkDto model)
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
        public async Task<ActionResult> LoadData([FromBody] DataManager request, [FromQuery] string colorGuid)
        {

            var data = await _service.LoadData(request, colorGuid);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
