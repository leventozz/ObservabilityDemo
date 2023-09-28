﻿using Microsoft.AspNetCore.Mvc;
using Order.API.OrderServices;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OrderCreateRequestDto requestDto)
        {
            await _orderService.CreateAsync(requestDto);
            return Ok(new OrderCreateResponseDto { Id= new Random().Next(1,500) });
        }
    }
}
