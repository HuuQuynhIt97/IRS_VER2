using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service)
        {
            _service = service;
        }

        #region Module

        [HttpGet("GetAllModule")]
        public async Task<IActionResult> GetAllModule()
        {
            var result = await _service.GetAllModule();
            return Ok(result);
        }
        [HttpGet("GetModulesAsTreeView")]
        public async Task<IActionResult> GetModulesAsTreeView()
        {
            var result = await _service.GetModulesAsTreeView();
            return Ok(result);
        }

        [HttpPost("CreateModule")]
        public async Task<IActionResult> CreateModule(ModuleDto create)
        {

            var result = await _service.AddModule(create);
            if (result.Success)
            {
                return NoContent();
            }

            throw new Exception("Creating the Module failed on save");
        }

        [HttpPut("UpdateModule")]
        public async Task<IActionResult> UpdateModule(ModuleDto update)
        {
            var result = await _service.UpdateModule(update);
            if (result.Success)
                return NoContent();
            return BadRequest($"Updating Module {update.ID} failed on save");
        }
        [HttpDelete("DeleteModule/{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var result = await _service.DeleteModule(id);
            if (result.Success)
                return NoContent();
            throw new Exception("Error deleting the Module");
        }

     
        #endregion

        #region Function

        [HttpGet("GetAllFunction")]
        public async Task<IActionResult> GetAllFunction()
        {
            var result = await _service.GetAllFunction();
            return Ok(result);
        }
        [HttpGet("GetFunctionsAsTreeView")]
        public async Task<IActionResult> GetFunctionsAsTreeView()
        {
            var result = await _service.GetFunctionsAsTreeView();
            return Ok(result);
        }
        [HttpPost("CreateFunction")]
        public async Task<IActionResult> CreateFunction(FunctionDto create)
        {
            var result = await _service.AddFunction(create);
            if (result.Success)
            {
                return NoContent();
            }

            throw new Exception("Creating the Function failed on save");
        }
        [HttpPut("UpdateFunction")]
        public async Task<IActionResult> UpdateFunction(FunctionDto update)
        {
            var result = await _service.UpdateFunction(update);
            if (result.Success)
                return NoContent();
            return BadRequest($"Updating Function {update.ID} failed on save");
        }

        [HttpDelete("DeleteFunction/{id}")]
        public async Task<IActionResult> DeleteFunction(int id)
        {
            var result = await _service.DeleteFunction(id);
            if (result.Success)
                return NoContent();
            throw new Exception("Error deleting the Function");
        }
     

        #endregion

        #region Action
        [HttpGet("GetAllAction")]
        public async Task<IActionResult> GetAllAction()
        {
            var result = await _service.GetAllAction();
            return Ok(result);
        }

        [HttpPost("CreateAction")]
        public async Task<IActionResult> CreateAction(Models.Action create)
        {
            var result = await _service.AddAction(create);
            if (result.Success)
            {
                return NoContent();
            }

            throw new Exception("Creating the Action failed on save");
        }

        [HttpPut("UpdateAction")]
        public async Task<IActionResult> UpdateAction(Models.Action update)
        {
            var result = await _service.UpdateAction(update);
            if (result.Success)
                return NoContent();
            return BadRequest($"Updating Action {update.ID} failed on save");
        }

        [HttpDelete("DeleteAction/{id}")]
        public async Task<IActionResult> DeleteAction(int id)
        {
            var result = await _service.DeleteAction(id);
            if (result.Success)
                return NoContent();
            throw new Exception("Error deleting the Action");
        }


        #endregion

        #region ActionInFunction

        [HttpPost("PostActionToFunction/{functionID}")]
        public async Task<IActionResult> PostActionToFunction(int functionID, ActionAssignRequest request)
        {
            //create new permission list from user changed

            var result = await _service.PostActionToFunction(functionID, request);
            if (result.Success)
            {
                return NoContent();
            }
            return BadRequest("Save ActionInFunction failed");
        }

        [HttpDelete("DeleteActionToFunction/{functionID}")]
        public async Task<IActionResult> DeleteActionToFunction(int functionID, [FromQuery] ActionAssignRequest request)
        {
            //create new permission list from user changed

            var result = await _service.DeleteActionToFunction(functionID, request);
            if (result.Success)
            {
                return NoContent();
            }
            return BadRequest("Save ActionInFunction failed");
        }
        #endregion

        #region Permission
       

        [HttpGet("GetMenuByLangID")]
        public async Task<IActionResult> GetMenuByLangID([FromQuery] int userID, [FromQuery] string langID)
        {
            //create new permission list from user changed

            var result = await _service.GetMenuByLangID(userID, langID);
            return Ok(result);
        }
        [HttpPost("GetScreenFunctionAndAction")]
        public async Task<IActionResult> GetScreenFunctionAndAction(ScreenFunctionAndActionRequest request)
        {
            //create new permission list from user changed

            var result = await _service.GetScreenFunctionAndAction(request);
            return Ok(result);
        }
        [HttpGet("GetActionInFunctionByRoleID/{roleID}")]
        public async Task<IActionResult> GetActionInFunctionByRoleID(int roleID)
        {
            var result = await _service.GetActionInFunctionByRoleID(roleID);
            return Ok(result);
        }
        [HttpGet("GetMenuByUserPermission/{userId}/{lang}")]
        public async Task<IActionResult> GetMenuByUserPermission(int userId, string lang)
        {
            var result = await _service.GetMenuByUserPermission(userId, lang);
            return Ok(result);
        }



        [HttpPut("PutPermissionByRoleId/{roleId}")]
        public async Task<IActionResult> PutPermissionByRoleId(int roleId, [FromBody] UpdatePermissionRequest request)
        {
            //create new permission list from user changed

            var result = await _service.PutPermissionByRoleId(roleId, request);
            if (result.Success)
            {
                return NoContent();
            }
            return BadRequest("Save permission failed");
        }

        [HttpGet("GetScreenAction/{functionID}")]
        public async Task<IActionResult> GetScreenAction(int functionID)
        {
            //create new permission list from user changed

            var result = await _service.GetScreenAction(functionID);
            return Ok(result);
        }

        #endregion

        [HttpPost("LoadData")]
        public async Task<ActionResult> LoadData([FromBody] DataManager request)
        {

            var data = await _service.LoadData(request);
            return Ok(data);
        }


        [HttpPost("LoadActionData")]
        public async Task<ActionResult> LoadActionData([FromBody] DataManager request)
        {

            var data = await _service.LoadActionData(request);
            return Ok(data);
        }

        [HttpPost("LoadModuleData")]
        public async Task<ActionResult> LoadModuleData([FromBody] DataManager request)
        {

            var data = await _service.LoadModuleData(request);
            return Ok(data);
        }

    }
}
