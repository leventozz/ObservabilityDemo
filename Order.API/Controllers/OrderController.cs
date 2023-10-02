﻿using Common.Shared.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.OrderServices;

namespace Order.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(OrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OrderCreateRequestDto requestDto)
        {
            var result = await _orderService.CreateAsync(requestDto);

            return new ObjectResult(result) { StatusCode = result.StatusCode };
        }

        [HttpGet]
        public async Task<IActionResult> SendOrderCreatedEvent()
        {
            await _publishEndpoint.Publish(new OrderCreatedEvent() { OrderCode = new Random().Next(1,100).ToString() });
            return Ok();
        }
    }
}
