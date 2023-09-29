using OpenTelemetry.Shared;
using Order.API.Models;
using System.Diagnostics;

namespace Order.API.OrderServices
{
    public class OrderService
    {
        private readonly AppDbContext _appDbContext;

        public OrderService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<OrderCreateResponseDto> CreateAsync(OrderCreateRequestDto requestDto)
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

            activity.SetTag("order user id", requestDto.UserId);
            activity.AddEvent(new ActivityEvent("Order process completed"));

            return new OrderCreateResponseDto() { Id = newOrder.Id };
        }
    }
}
