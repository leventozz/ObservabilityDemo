using Order.API.OrderServices;

namespace Order.API.OrderService
{
    public class OrderService
    {
        public Task CreateAsync(OrderCreateRequestDto requestDto)
        {
            return Task.CompletedTask;
        }
    }
}
