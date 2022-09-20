using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class SystemLanguageController : ApiControllerBase
    {
        private readonly ISystemLanguageService _service;

        public SystemLanguageController(ISystemLanguageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult> LoadLanguages()
        //{
        //    return Ok(await _service.LoadLanguages());
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetLanguages(string lang)
        {
            return Ok(await _service.GetLanguages(lang));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> LoadLanguages(string lang)
        {
            return Ok(await _service.LoadLanguages(lang));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] SystemLanguageDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] SystemLanguageDto model)
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
        public async Task<ActionResult> GetPages(string lang)
        {
            return Ok(await _service.GetPages(lang));
        }

        [HttpGet]
        public async Task<ActionResult> GetTypes(string lang)
        {
            return Ok(await _service.GetTypes(lang));
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoadData(DataManager data, string page, string type)
        {
            return Ok(await _service.LoadData(data, page, type));
        }

        [HttpPost]
        public async Task<ActionResult> LoadReportColumnData(DataManager data, string systemMenuGuid)
        {
            return Ok(await _service.LoadReportColumnData(data, systemMenuGuid));
        }
        [HttpGet]
        public async Task<ActionResult> GetReportColumns(string menuLink, string lang)
        {
            return Ok(await _service.GetReportColumns(menuLink, lang));
        }
        [HttpGet]
        public async Task<ActionResult> UpdateBySequence(string systemMenuGuid, decimal fromSequence, decimal dropSequence)
        {
            return StatusCodeResult(await _service.UpdateBySequence(systemMenuGuid, fromSequence, dropSequence));
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
