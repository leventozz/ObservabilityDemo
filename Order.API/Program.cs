using Observability.Order.API.OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenTelemetryConstants>(builder.Configuration.GetSection("OpenTelemetry"));



builder.Services.AddOpenTelemetry().WithTracing(options =>
{
    var OTConstants = (builder.Configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>())!;
    options.AddSource(OTConstants.ActivitySourceName)
    .ConfigureResource(resource =>
    {
        resource.AddService(OTConstants.ServiceName, serviceVersion: OTConstants.ServiceVersion);
    });
    options.AddAspNetCoreInstrumentation();
    options.AddConsoleExporter();
    options.AddOtlpExporter();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
