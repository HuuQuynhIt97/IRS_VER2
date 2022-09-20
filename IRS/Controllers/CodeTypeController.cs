using Microsoft.AspNetCore.Mvc;

using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class CodeTypeController : ApiControllerBase
    {
        private readonly ICodeTypeService _service;

        public CodeTypeController(ICodeTypeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetCodeTypes()
        {
            var top = HttpContext.Request.Query["$top"].ToInt();
            var skip = HttpContext.Request.Query["$skip"].ToInt();
            var search = HttpContext.Request.Query["search"].ToSafetyString();
            var selected = HttpContext.Request.Query["selected"].ToSafetyString();
            var codeType = HttpContext.Request.Query["codeType"].ToSafetyString();
            var lang = HttpContext.Request.Query["lang"].ToSafetyString();
            return Ok(await _service.GetCodeTypes(codeType, top, skip, search, selected, lang));
        }
        [HttpPost]
        public async Task<ActionResult> GetDataDropdownlist([FromBody] DataManager request, string lang, string codeType)
        {
            return Ok(await _service.GetDataDropdownlist(request, lang, codeType));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CodeTypeDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] CodeTypeDto model)
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
        [HttpGet]
        public async Task<ActionResult> GetCodeTypesDropdownlist()
        {
            return Ok(await _service.GetCodeTypes());
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }

        
    }
}
