using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System;
using System.Threading.Tasks;

using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace IRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ColorWorkPlanController : ControllerBase
    {
        private readonly IColorWorkPlanService _service;

        public ColorWorkPlanController(IColorWorkPlanService service)
        {
            _service = service;
        }

        [NonAction] //Set not Tracking http method
        public ObjectResult StatusCodeResult(OperationResult result)
        {
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("LoadDataColorWorkPlan")]
        public async Task<ActionResult> LoadDataColorWorkPlan([FromBody] DataManager request)
        {

            var data = await _service.LoadDataColorWorkPlan(request);
            return Ok(data);
        }

        [HttpPost("LoadDataColorWorkPlan2")]
        public async Task<ActionResult> LoadDataColorWorkPlan2()
        {

            var data = await _service.LoadDataColorWorkPlan2();
            return Ok(data);
        }


        [HttpPost("AddColorWorkPlan")]
        public async Task<ActionResult> AddColorWorkPlan([FromBody] ColorWorkPlanDto model)
        {
            return StatusCodeResult(await _service.AddColorWorkPlan(model));
        }

        [HttpPut("UpdateColorWorkPlan")]
        public async Task<ActionResult> UpdateColorWorkPlan([FromBody] ColorWorkPlanDto model)
        {
            return StatusCodeResult(await _service.UpdateColorWorkPlan(model));
        }

        [HttpGet("LoadShoes")]
        public async Task<ActionResult> LoadShoes()
        {
            var data = await _service.LoadShoes();
            return Ok(data);
        }

        // [HttpDelete("DeleteColorWorkPlan/{id}")]
        // public async Task<IActionResult> DeleteColorWorkPlan(int id)
        // {
        //     var result = await _service.DeleteColorWorkPlan(id);
        //     if (result.Success)
        //         return NoContent();
        //     throw new Exception("Error deleting");
        // }

        [HttpDelete("DeleteColorWorkPlan/{id}")]
        public async Task<ActionResult> DeleteColorWorkPlan(int id)
        {
            return StatusCodeResult(await _service.DeleteColorWorkPlan(id));
        }

        [HttpGet("StoreProcedureCreateColorTodo")]
        public async Task<object> StoreProcedureCreateColorTodo(DateTime currentDate)
        {
            return await _service.StoreProcedureCreateColorTodo(currentDate);
        }

        [HttpGet("LoadColorToDo")]
        public async Task<ActionResult> LoadColorToDo()
        {
            var data = await _service.LoadColorToDo();
            return Ok(data);
        }

        [HttpPut("UpdateIsFinishedColorToDo")]
        public async Task<ActionResult> UpdateIsFinishedColorToDo(ColorTodoDto model)
        {
            return StatusCodeResult(await _service.UpdateIsFinishedColorToDo(model));
        }

        [HttpPut("UpdateColorToDoAmount")]
        public async Task<ActionResult> UpdateColorToDoAmount(ColorTodoDto model)
        {
            return StatusCodeResult(await _service.UpdateColorToDoAmount(model));
        }

    }
}
