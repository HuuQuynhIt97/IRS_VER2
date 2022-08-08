using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using System;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class MethodController : ApiControllerBase
    {
        private readonly IMethodService _service;

        public MethodController(IMethodService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] MethodDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] MethodDto model)
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

    }
}
