using Common.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payment.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create(PaymentCreateRequestDto request)
        {
            const decimal balance = 1000;

            if(request.TotalPrice > balance)
                return BadRequest(ResponseDto<PaymentCreateResponseDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(), "Insufficient balance!"));

            return Ok(ResponseDto<PaymentCreateResponseDto>.Success(HttpStatusCode.OK.GetHashCode(), new PaymentCreateResponseDto()
            {
                Description="Process completed!"
            }));
        }
    }
}
