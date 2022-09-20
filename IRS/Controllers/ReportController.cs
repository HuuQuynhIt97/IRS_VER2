using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Services;
using System;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class ReportController : ApiControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetReportType(string menuLink)
        {
            return Ok(await _service.GetReportType(menuLink));
        }
       
        [HttpGet]
        public async Task<ActionResult> GetReports([FromQuery]ReportParams reportParams)
        {
            return Ok(await _service.GetReport(reportParams));
        }
        [HttpGet]
        public async Task<ActionResult> GetReportChart(DateTime d1, DateTime d2 ,string menuLink, string lang)
        {
            return Ok(await _service.GetReportChart(d1, d2, menuLink, lang));
        }
        [HttpGet]
        public async Task<ActionResult> GetReportChartSetting(string menuLink, string lang)
        {
            return Ok(await _service.GetReportChartSetting(menuLink, lang));
        }
       
        [HttpPost]
        public IActionResult ExcelExport(ExcelExportChartDto model)
        {
            var bin = _service.ExcelExport(model);
            return File(bin, "application/octet-stream", $"{model.FunctionName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx");
        }
        [HttpPost]
        public IActionResult ExcelExportPieChart(ExcelExportPieChartDto model)
        {
            var bin = _service.ExcelExportPieChart(model);
            return File(bin, "application/octet-stream", $"{model.FunctionName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx");
        }

    }
}
