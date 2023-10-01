﻿using Common.Shared.DTOs;
using OpenTelemetry.Shared;
using Order.API.Models;
using Order.API.StockServices;
using System.Diagnostics;
using System.Net;

namespace Order.API.OrderServices
{
    public class OrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly StockService _stockService;
        public OrderService(AppDbContext appDbContext, StockService stockService)
        {
            _appDbContext = appDbContext;
            _stockService = stockService;
        }

        public async Task<ResponseDto<OrderCreateResponseDto>> CreateAsync(OrderCreateRequestDto requestDto)
        {
            using var activity = ActivitySourceProvider.Source.StartActivity()!;
            activity.AddEvent(new ActivityEvent("Order process started"));

            var newOrder = new Order()
            {
                Created = DateTime.Now,
                OrderCode = Guid.NewGuid().ToString(),
                Status = OrderStatus.Succes,
                UserId = requestDto.UserId,
                Items = requestDto.Items.Select(x => new OrderItem()
                {
                    Count = x.Count,
                    UnitPrice = x.UnitPrice,
                    ProductId = x.ProductId
                }).ToList()
            };

            _appDbContext.Orders.Add(newOrder);
            await _appDbContext.SaveChangesAsync();


            StockCheckAndPaymentProcessRequestDto stockCheckAndPaymentProcessRequest = new()
            {
                OrderCode = newOrder.OrderCode,
                OrderItems = requestDto.Items
            };
            var (isSuccess, failMessage) = await _stockService.StockCheckAndPaymentStartAsync(stockCheckAndPaymentProcessRequest);

            if (!isSuccess)
                return ResponseDto<OrderCreateResponseDto>.Fail(HttpStatusCode.InternalServerError.GetHashCode(),failMessage!);

            activity.AddEvent(new ActivityEvent("Order process completed"));

            return ResponseDto<OrderCreateResponseDto>.Success(HttpStatusCode.OK.GetHashCode(), new OrderCreateResponseDto() { Id = newOrder.Id });
        }
    }
}
