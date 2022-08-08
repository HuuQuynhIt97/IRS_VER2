using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Helpers;
using IRS.Models;
using IRS.Services;
using Syncfusion.JavaScript;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class RoomController : ApiControllerBase
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUploadFile([FromForm] decimal key)
        {
            return Ok(await _service.DeleteUploadFile(key));
        }
        [HttpPost]
        public async Task<ActionResult> AddFormAsync([FromForm] RoomDto model)
        {
            return Ok(await _service.AddFormAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFormAsync([FromForm] RoomDto model)
        {
            return Ok(await _service.UpdateFormAsync(model));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult> GetRoomByID(int ID)
        {
            return Ok(await _service.GetRoomByid(ID));
        }
        [HttpPost]
        public async Task<ActionResult> Search(RoomFilterDto filter)
        {
            return Ok(await _service.Search(filter));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] RoomDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync([FromBody] RoomDto model)
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
        public async Task<ActionResult> GetRoomBySiteAndHall(string siteID , string hallID)
        {
            return Ok(await _service.GetRoomBySiteAndHall(siteID, hallID));
        }
        [HttpGet]
        public async Task<ActionResult> GetRooms()
        {
            var top = HttpContext.Request.Query["$top"].ToInt();
            var skip = HttpContext.Request.Query["$skip"].ToInt();
            var farmGuid = HttpContext.Request.Query["farmGuid"].ToSafetyString();
            var search = HttpContext.Request.Query["search"].ToSafetyString();
            var selected = HttpContext.Request.Query["selected"].ToSafetyString();
            return Ok(await _service.GetRooms(farmGuid, top, skip, search, selected));
        }
        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPost]
        public async Task<ActionResult> LoadData([FromBody] DataManager request, [FromQuery] string siteGuid, [FromQuery] string hallGuid)
        {

            var data = await _service.LoadData(request, siteGuid, hallGuid);
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
