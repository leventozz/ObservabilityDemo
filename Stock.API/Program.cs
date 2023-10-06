using Common.Shared;
using Logging.Shared;
using MassTransit;
using OpenTelemetry.Shared;
using Serilog;
using Stock.API;
using Stock.API.Consumers;
using Stock.API.PaymentServices;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(LoggingExt.ConfigureLogging);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddOpenTelemetryExt(builder.Configuration);
builder.Services.AddHttpClient<PaymentService>(options =>
{
    options.BaseAddress = new Uri((builder.Configuration.GetSection("ApiServices")["PaymentApi"])!);
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        cfg.ReceiveEndpoint("stock.order-created-event.queue", e=>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
