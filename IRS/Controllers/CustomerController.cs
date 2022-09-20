using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CustomerDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] CustomerDto model)
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
        public async Task<ActionResult> LoadData([FromBody] DataManager request, string farmGuid, string lang)
        {

            var data = await _service.LoadData(request,farmGuid, lang);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            var top = HttpContext.Request.Query["$top"].ToInt();
            var skip = HttpContext.Request.Query["$skip"].ToInt();
            var farmGuid = HttpContext.Request.Query["farmGuid"].ToSafetyString();
            var search = HttpContext.Request.Query["search"].ToSafetyString();
            var selected = HttpContext.Request.Query["selected"].ToSafetyString();
            return Ok(await _service.GetCustomers(farmGuid, top, skip, search, selected));
        }
        //[HttpGet]
        //public async Task<ActionResult> GetCustomers(string farmGuid)
        //{
        //    return Ok(await _service.GetCustomers(farmGuid));
        //}
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
