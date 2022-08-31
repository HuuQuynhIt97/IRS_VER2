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

namespace IRS.Controllers
{
    public class ScheduleController : ApiControllerBase
    {
        private readonly IScheduleService _service;

        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Import([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            var datasList = new List<ScheduleUploadDto>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
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
                            ArticelNo = workSheet.Cells[rowIterator, 5].Value.ToSafetyString(),
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
        public async Task<ActionResult> AddAsync([FromBody] ScheduleDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] ScheduleDto model)
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
