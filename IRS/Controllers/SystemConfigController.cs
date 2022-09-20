
using Microsoft.AspNetCore.Mvc;

using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class SystemConfigController : ApiControllerBase
    {
        private readonly ISystemConfigService _service;

        public SystemConfigController(ISystemConfigService service)
        {
            _service = service;
        }
      
       
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult> LoadDataSystem()
        {
            return Ok(await _service.LoadDataSystem());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] SystemConfigDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] SystemConfigDto model)
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
        public async Task<ActionResult> LoadData([FromBody] DataManager request)
        {

            var data = await _service.LoadData(request);
            return Ok(data);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
          [HttpPost]
        public async Task<ActionResult> GetDataDropdownlist([FromBody] DataManager request)
        {
           
            return Ok(await _service.GetDataDropdownlist(request));
        }
    }
}
