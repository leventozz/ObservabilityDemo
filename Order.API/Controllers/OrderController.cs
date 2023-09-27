using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Create()
        {
            int a = 5;
            int b = 0;
            int c = a / b;
            return Ok();
        }
    }
}
