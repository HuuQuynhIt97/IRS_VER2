using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;
using System.Net;
using System;
using System.IO;

namespace IRS.Controllers
{
    public class BookingController : ApiControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpPost]
        public async Task<ActionResult> Search(BookingFilterDto filter)
        {
            return Ok(await _service.Search(filter));
        }

        [HttpPost]
        public async Task<ActionResult> SearchDetailRoom(BookingFilterDto filter)
        {
            return Ok(await _service.SearchDetailRoom(filter));
        }

        [HttpPost]
        public async Task<ActionResult> SearchBookingCheck(BookingCheckFilterDto filter)
        {
            return Ok(await _service.SearchBookingCheck(filter));
        }

        [HttpGet]
        public bool DoesImageExistRemotely(string uriToImage)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + uriToImage);
            var check_exist_file = System.IO.File.Exists(path);
            return check_exist_file;
            
        }

        [HttpPost]
        public async Task<ActionResult> GetSearchBookingCalendar(BookingCheckFilterDto filter)
        {
            return Ok(await _service.GetSearchBookingCalendar(filter));
        }

        [HttpPost]
        public async Task<ActionResult> GetSearchByList(BookingCheckFilterDto filter)
        {
            return Ok(await _service.GetSearchByList(filter));
        }

        [HttpGet]
        public async Task<ActionResult> GetPreviewScheduler(string siteGuid)
        {
            return Ok((await _service.GetPreviewScheduler(siteGuid)));
        }

        [HttpPost]
        public async Task<ActionResult> GetPreviewScheduler([FromBody] DataManager request, [FromQuery] string siteGuid)
        {

            var data = await _service.GetPreviewScheduler(request, siteGuid);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] BookingDetailDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] BookingDetailDto model)
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


        [HttpGet]
        public async Task<ActionResult> GetBookingCheck()
        {
            return Ok(await _service.GetBookingCheck());
        }

        [HttpGet]
        public async Task<ActionResult> GetByHall(string hallID)
        {
            return Ok(await _service.GetByHall(hallID));
        }
    }
}
