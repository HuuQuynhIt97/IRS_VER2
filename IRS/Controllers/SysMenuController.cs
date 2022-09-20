using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class SysMenuController : ApiControllerBase
    {
        private readonly ISysMenuService _service;

        public SysMenuController(ISysMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] SysMenuDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] SysMenuDto model)
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
        public async Task<ActionResult> GetMenus(string lang)
        {
            return Ok(await _service.GetMenus(lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetMenusByFarm(string lang, string farmGuid)
        {
            return Ok(await _service.GetMenusByFarm(farmGuid,lang));
        }
        [HttpPost]
        public async Task<ActionResult> LoadData([FromBody] DataManager request)
        {

            var data = await _service.LoadData(request);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> LoadData2([FromBody] DataManager request, decimal upperId)
        {

            var data = await _service.LoadData(request, upperId);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> LoadReportMenuData([FromBody] DataManager request, string reportType)
        {

            var data = await _service.LoadReportMenuData(request, reportType);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> LoadReportChartConfigMenuData([FromBody] DataManager request, string reportType)
        {

            var data = await _service.LoadReportChartConfigMenuData(request, reportType);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetParents(string lang)
        {
            return Ok(await _service.GetParents(lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetToolbarParents(string lang)
        {
            return Ok(await _service.GetToolbarParents(lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetItemByKind([FromQuery] string lang, [FromQuery] string kind)
        {
            return Ok(await _service.GetItemByKind(lang, kind));
        }

        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
