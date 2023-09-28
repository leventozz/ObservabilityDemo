using Observability.Order.API.OpenTelemetry;
using Order.API.OrderServices;
using System.Diagnostics;

namespace Order.API.OrderService
{
    public class OrderService
    {
        public Task CreateAsync(OrderCreateRequestDto requestDto)
        {
            using var activity = ActivitySourceProvider.Source.StartActivity()!;
            activity.AddEvent(new ActivityEvent("Order process started"));

            //Do some db operations etc.

            activity.AddEvent(new ActivityEvent("Order process completed"));

            return Task.CompletedTask;
        }
    }
}
