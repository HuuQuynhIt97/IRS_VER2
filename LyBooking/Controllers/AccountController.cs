using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            return Ok(await _service.GetAccounts());
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] AccountDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] AccountDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddFormAsync([FromForm] AccountDto model,[FromQuery] List<int> OCIDList)
        {
            model.OCIDList = OCIDList;
            return Ok(await _service.AddFormAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFormAsync([FromForm] AccountDto model, [FromQuery] List<int> OCIDList)
        {
            model.OCIDList = OCIDList;
            return Ok(await _service.UpdateFormAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> LockAsync(int id)
        {
            return StatusCodeResult(await _service.LockAsync(id));
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
        [HttpPost]
        public async Task<ActionResult> DeleteUploadFile([FromForm] int key)
        {
            return Ok(await _service.DeleteUploadFile(key));
        }
        [HttpPost]
        public async Task<ActionResult> ShowPassword([FromForm] int key)
        {
            return Ok(await _service.ShowPassword(key));
        }


        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            return StatusCodeResult(await _service.ChangePassword(changePassword));
        }
    }
}
