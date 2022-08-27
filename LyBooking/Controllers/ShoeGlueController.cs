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

        [HttpPost]
        public async Task<ActionResult> Import([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            var datasList = new List<ScheduleUploadDto>();
            var result = new List<ScheduleUploadDto>();
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
                            ScheduleId = workSheet.Cells[rowIterator, 2].Value.ToInt(),
                            ModelName = workSheet.Cells[rowIterator, 3].Value.ToSafetyString(),
                            ModelNo = workSheet.Cells[rowIterator, 4].Value.ToSafetyString(),
                            ArticleNo = workSheet.Cells[rowIterator, 5].Value.ToSafetyString(),
                            Treatment = workSheet.Cells[rowIterator, 6].Value.ToSafetyString(),
                            Process = workSheet.Cells[rowIterator, 7].Value.ToSafetyString(),
                            GlueID = workSheet.Cells[rowIterator, 8].Value.ToInt(),
                            PartID = workSheet.Cells[rowIterator, 9].Value.ToSafetyString(),
                            Name = workSheet.Cells[rowIterator, 10].Value.ToSafetyString() == "NULL" ? "" : workSheet.Cells[rowIterator, 10].Value.ToSafetyString(),
                            Consumption = workSheet.Cells[rowIterator, 11].Value.ToSafetyString() == "NULL" ? "" : workSheet.Cells[rowIterator, 11].Value.ToSafetyString(),
                            TreatmentWayID = workSheet.Cells[rowIterator, 12].Value.ToInt(),
                            Status = 1
                        });
                    }

                    result = datasList.Where(x => x.TreatmentWayID != 0 && x.Name != "1" && x.Name != "").ToList();
                }
                
                await _service.ImportExcel(result);
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }
    }
}
