using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IRS.Controllers
{
    public class WorkPlan2Controller : ApiControllerBase
    {
        private readonly IWorkPlan2Service _service;

        public WorkPlan2Controller(IWorkPlan2Service service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllWorkPlan2() {
            var data = await _service.GetAllWorkPlan2();
            return Ok(data);
        }        

        [HttpPost]
        public async Task<ActionResult> ImportExcelWorkPlan2([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            var datasList = new List<WorkPlan2Dto>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    var currentTime = DateTime.Now;
                    var CutStartDateee = workSheet.Cells["AV6"].Value.ToInt();
                    var datetimee = DateTime.FromOADate(CutStartDateee);
                    
                    for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                    {
                        var CutStart = workSheet.Cells["AV" + rowIterator.ToString()].Value.ToInt();
                        var CutStartDate = string.Empty;
                        if (CutStart > 0)
                        {
                            CutStartDate = DateTime.FromOADate(CutStart).Month.ToString() + "/" + DateTime.FromOADate(CutStart).Day.ToString();
                        }

                        var SFStart = workSheet.Cells["BB" + rowIterator.ToString()].Value.ToInt();
                        var SFStartDate = string.Empty;
                        if (SFStart > 0)
                        {
                            SFStartDate = DateTime.FromOADate(SFStart).Month.ToString() + "/" + DateTime.FromOADate(SFStart).Day.ToString();
                        }
                        datasList.Add(new WorkPlan2Dto()
                        {   
                            StTeam = workSheet.Cells["E" + rowIterator.ToString()].Value.ToSafetyString(),
                            SfTeam = workSheet.Cells["F" + rowIterator.ToString()].Value.ToSafetyString(),
                            Pono = workSheet.Cells["R" + rowIterator.ToString()].Value.ToSafetyString(),
                            Batch = workSheet.Cells["S" + rowIterator.ToString()].Value.ToSafetyString(),
                            ModelName = workSheet.Cells["X" + rowIterator.ToString()].Value.ToSafetyString(),
                            ModelNo = workSheet.Cells["Y" + rowIterator.ToString()].Value.ToSafetyString(),
                            ArticleNo = workSheet.Cells["Z" + rowIterator.ToString()].Value.ToSafetyString(),
                            Qty = workSheet.Cells["AA" + rowIterator.ToString()].Value.ToSafetyString(),
                            CutStartDate = CutStartDate,
                            SfStartDate = SFStartDate,
                            ScheduleId = 0,
                            Status = true,
                            CreateDate = currentTime
                        });
                    }
                }
                await _service.ImportExcelWorkPlan2(datasList);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetBuyingList() {
            var data = await _service.GetBuyingList();
            return Ok(data);
        }

        [HttpPost("{lang}")]
        public async Task<IActionResult> ExportExcelBuyingList(string lang)
        {
            var bin = await _service.ExportExcelBuyingList(lang);
            return File(bin, "application/octet-stream", "ExportExcelBuyingList.xlsx");
        }
             
    }
}
