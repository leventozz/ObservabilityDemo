using Microsoft.AspNetCore.Mvc;

namespace Stock.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public async Task<IActionResult> StockCheckAndPaymentStart()
        {
            
        }
    }
}
