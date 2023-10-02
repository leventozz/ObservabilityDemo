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
        public async Task<IActionResult> StockCheckAndPaymentStart(StockCheckAndPaymentProcessRequestDto requestDto)
        {
            var result = await _stockService.CheckAndPaymentProcessAsync(requestDto);
            return new ObjectResult(result) { StatusCode = result.StatusCode };
        }
    }
}
