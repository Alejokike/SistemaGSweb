using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGS.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ResourceController : Controller
    {
        [Authorize]
        [HttpGet("Verify")]
        public ActionResult Verify()
        {
            return Ok("You are Authorized!!!");
        }
    }
}
