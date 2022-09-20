
using Microsoft.AspNetCore.Mvc;
using IRS.DTO;

namespace IRS.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiControllerBase: ControllerBase
    {
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
    }
}
