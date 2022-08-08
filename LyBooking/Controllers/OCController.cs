using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using System;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class OCController : ApiControllerBase
    {
        private readonly IOCService _service;

        public OCController(IOCService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetFarms()
        {
            return Ok(await _service.GetFarms());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsTreeView()
        {
            return Ok(await _service.GetAllAsTreeView());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsTreeViewByFarm(int farmID)
        {
            return Ok(await _service.GetAllAsTreeView(farmID));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] OCDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromForm] OCDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return Ok(await _service.DeleteAsync(id));
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

        [HttpPost]
        public async Task<IActionResult> CreateSubOC(OCDto create)
        {
            return Ok(await _service.CreateSubOC(create));
        }
        [HttpPost]
        public async Task<IActionResult> CreateMainOC(OCDto create)
        {
            var res = await _service.CreateMainOC(create);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSubOCFormAsync([FromForm] OCDto model)
        {
            return Ok(await _service.CreateSubOCForm(model));
        }
        [HttpPost]
        public async Task<ActionResult> CreateMainOCFormAsync([FromForm] OCDto model)
        {
            return Ok(await _service.CreateMainOCForm(model));
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUploadFile([FromForm] int key)
        {
            return Ok(await _service.DeleteUploadFile(key));
        }
    }
}
