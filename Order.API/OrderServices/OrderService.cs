using OpenTelemetry.Shared;
using System.Diagnostics;

namespace Order.API.OrderServices
{
    public class OrderService
    {
        public Task CreateAsync(OrderCreateRequestDto requestDto)
        {
            using var activity = ActivitySourceProvider.Source.StartActivity()!;
            activity.AddEvent(new ActivityEvent("Order process started"));

            //Do some db operations etc.

            activity.SetTag("order user id", requestDto.UserId);

            activity.AddEvent(new ActivityEvent("Order process completed"));

            return Task.CompletedTask;
        }
    }
}
