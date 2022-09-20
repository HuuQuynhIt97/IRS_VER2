using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IRS.Controllers
{
    public class StockInChemicalController : ApiControllerBase
    {
        private readonly IStockInChemicalService _service;

        public StockInChemicalController(IStockInChemicalService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<ActionResult> DataFiterExecuteAndCreate(StockInInkFilterRequestDto filter)
        {
            return Ok(await _service.DataFiterExecuteAndCreate(filter));
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
        public async Task<ActionResult> AddAsync([FromBody] StockInChemicalDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] StockInChemicalDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateExecution(List<StockInChemicalDto> model)
        {
            return Ok(await _service.UpdateExecution(model));
        }



        [HttpPost]
        public async Task<ActionResult> DeleteAsync(decimal id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Approve(string guid)
        {
            return Ok(await _service.Approve(guid));
        }

        [HttpPost]
        public async Task<ActionResult> UnApprove(string guid)
        {
            return Ok(await _service.UnApprove(guid));
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
        public async Task<ActionResult> LoadData([FromQuery] string colorGuid)
        {

            var data = await _service.LoadData(colorGuid);
            return Ok(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetAudit(decimal id)
        {
            return Ok(await _service.GetAudit(id));
        }
    }
}
