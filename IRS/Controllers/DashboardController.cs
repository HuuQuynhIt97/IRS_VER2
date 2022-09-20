using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class DashboardController : ApiControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }
        [HttpGet]
        // public async Task<ActionResult> GetDashboards()
        // {
        //     var top = HttpContext.Request.Query["$top"].ToInt();
        //     var skip = HttpContext.Request.Query["$skip"].ToInt();
        //     var search = HttpContext.Request.Query["search"].ToSafetyString();
        //     var selected = HttpContext.Request.Query["selected"].ToSafetyString();
        //     var codeType = HttpContext.Request.Query["codeType"].ToSafetyString();
        //     var lang = HttpContext.Request.Query["lang"].ToSafetyString();
        //     return Ok(await _service.GetDashboards(codeType, top, skip, search, selected, lang));
        // }
        [HttpGet]
        public async Task<ActionResult> GetAreas()
        {
            var top = HttpContext.Request.Query["$top"].ToInt();
            var skip = HttpContext.Request.Query["$skip"].ToInt();
            var search = HttpContext.Request.Query["search"].ToSafetyString();
            var selected = HttpContext.Request.Query["selected"].ToSafetyString();
            var codeType = HttpContext.Request.Query["codeType"].ToSafetyString();
            var lang = HttpContext.Request.Query["lang"].ToSafetyString();
            return Ok(await _service.GetAreas(codeType, top, skip, search, selected, lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] DashboardDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] DashboardDto model)
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
        public async Task<ActionResult> LoadData([FromBody] DataManager request, string codeType)
        {

            var data = await _service.LoadData(request, codeType);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> LoadDashboardData([FromBody] DataManager request, string farmGuid)
        {

            var data = await _service.LoadDashboardData(request, farmGuid);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> LoadAreaData([FromBody] DataManager request, string dashboardGuid)
        {

            var data = await _service.LoadAreaData(request, dashboardGuid);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> LoadItemData([FromBody] DataManager request, string dashboardGuid, string areaGuid, [FromQuery]List<string> type)
        {

            var data = await _service.LoadItemData(request, dashboardGuid, areaGuid, type);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetDashboardsDropdownlist()
        {
            return Ok(await _service.GetDashboards());
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }

         [HttpGet]
        public async Task<ActionResult> GetDashboards(string dashboardGuid, DateTime yearMonth, string lang)
        {
            return Ok(await _service.GetDashboards(dashboardGuid, yearMonth, lang));
        }
         [HttpGet]
        public async Task<ActionResult> GetDashboardsNav(string farmGuid)
        {
            return Ok(await _service.GetDashboardsNav(farmGuid));
        }
    }
}
