using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IRS.Controllers
{

    public class EmployeeController : ApiControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
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

        #region CURD, sort, filter, EJ2 Grid UrlAdaptor
        [HttpPost]
        public async Task<ActionResult> LoadDataByFarm([FromBody] DataManager request,string farmGuid, string lang)
        {

            var data = await _service.LoadData(request,farmGuid, lang);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> LoadData([FromBody] DataManager request, string lang)
        {

            var data = await _service.LoadData(request, lang);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CRUDModel<EmployeeDto> model)
        {
            return StatusCodeResult(await _service.AddAsync(model.Value));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] CRUDModel<EmployeeDto> model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model.Value));
        }
        #endregion
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            return Ok(await _service.GetEmployees());
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesByAccountID(int accountID)
        {
            return Ok(await _service.GetEmployeesByAccountID(accountID));
        }
        [HttpGet]
        public async Task<IActionResult> ExcelExport()
        {
            string filename = "employee.xlsx";
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/excelTemplate", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/octet-stream"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
         [HttpGet]
        public async Task<ActionResult> CheckExist(string no)
        {
            return Ok(await _service.CheckExist(no));
        }
    }
}
