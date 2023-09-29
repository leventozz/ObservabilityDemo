using Common.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Stock.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }
        [HttpPost]
        public  IActionResult StockCheckAndPaymentStart(StockCheckAndPaymentProcessRequestDto requestDto)
        {
            var result = _stockService.CheckAndPaymentProcess(requestDto);
            return new ObjectResult(result) { StatusCode = result.StatusCode };
        }
    }
}
