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
    public class WorkPlanController : ApiControllerBase
    {
        private readonly IWorkPlanService _service;

        public WorkPlanController(IWorkPlanService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWorkPlan()
        {
            var workplans = await _service.GetAllWorkPlan();
            return Ok(workplans);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkPlanNew()
        {
            var workplanNew = await _service.GetAllWorkPlanNew();
            return Ok(workplanNew);
        }

        [HttpPost]
        public async Task<ActionResult> Import([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            var datasList = new List<ScheduleUploadDto>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        datasList.Add(new ScheduleUploadDto()
                        {
                            ScheduleID = workSheet.Cells[rowIterator, 2].Value.ToInt(),
                            ModelName = workSheet.Cells[rowIterator, 3].Value.ToSafetyString(),
                            ModelNo = workSheet.Cells[rowIterator, 4].Value.ToSafetyString(),
                            ArticelNo = workSheet.Cells[rowIterator, 5].Value. ToSafetyString(),
                            Treatment = workSheet.Cells[rowIterator, 6].Value.ToSafetyString(),
                            Process = workSheet.Cells[rowIterator, 7].Value.ToSafetyString(),
                            GlueID = workSheet.Cells[rowIterator, 8].Value.ToInt(),
                            Part = workSheet.Cells[rowIterator, 9].Value.ToString(),
                            Name = workSheet.Cells[rowIterator, 10].Value.ToString(),
                            Consumption = workSheet.Cells[rowIterator, 11].Value.ToString(),
                            TreatmentWayID = workSheet.Cells[rowIterator, 12].Value.ToInt(),
                        });
                    }
                }
                
                await _service.ImportExcel(datasList);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        public async Task<ActionResult> ImportWorkPlanNew([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            var datasList = new List<WorkPlanNewDto>();
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

                    for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                    {
                        datasList.Add(new WorkPlanNewDto()
                        {   
                            Line = workSheet.Cells["A" + rowIterator.ToString()].Value.ToSafetyString(),
                            Line2 = workSheet.Cells["B" + rowIterator.ToString()].Value.ToSafetyString(),
                            Pono = workSheet.Cells["I" + rowIterator.ToString()].Value.ToSafetyString(),
                            ModelName = workSheet.Cells["K" + rowIterator.ToString()].Value.ToSafetyString(),
                            ModelNo = workSheet.Cells["L" + rowIterator.ToString()].Value.ToSafetyString(),
                            ArticleNo = workSheet.Cells["M" + rowIterator.ToString()].Value.ToSafetyString(),
                            Qty = workSheet.Cells["N" + rowIterator.ToString()].Value.ToSafetyString(),
                            Treatment = workSheet.Cells["S" + rowIterator.ToString()].Value.ToSafetyString(),
                            ScheduleId = 0,
                            Status = true,
                            CreateDate = currentTime
                        });
                    }
                }
                await _service.ImportExcelWorkPlanNew(datasList);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

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
        public async Task<ActionResult> AddAsync([FromBody] WorkPlanDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] WorkPlanDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetByIDAsync(int id)
        {
            return Ok(await _service.GetByIDAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }

        [HttpGet]
        public async Task<ActionResult> LoadData(string shoeGuid, string lang)
        {

            var data = await _service.LoadData(shoeGuid, lang);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
