using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OpenTelemetry.Shared
{
    public static class OpenTelemetryExtension
    {
        public static void AddOpenTelemetryExt(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<OpenTelemetryConstants>(configuration.GetSection("OpenTelemetry"));
            var OTConstants = (configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>())!;
            ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(OTConstants.ActivitySourceName);

            services.AddOpenTelemetry().WithTracing(options =>
            {
                options.AddSource(OTConstants.ActivitySourceName)
                .ConfigureResource(resource =>
                {
                    resource.AddService(OTConstants.ServiceName, serviceVersion: OTConstants.ServiceVersion);
                });

                options.AddAspNetCoreInstrumentation(instrumentationsOptions =>
                {
                    instrumentationsOptions.Filter = (context) =>
                    {
                        return context.Request.Path.Value!.Contains("api", StringComparison.InvariantCulture);
                    };
                    instrumentationsOptions.RecordException = true;
                });

                options.AddEntityFrameworkCoreInstrumentation(efcoreOptions =>
                {
                    efcoreOptions.SetDbStatementForText = true;
                    efcoreOptions.SetDbStatementForStoredProcedure = true;
                });

                options.AddHttpClientInstrumentation(httpOptions =>
                {
                    httpOptions.EnrichWithHttpRequestMessage = async (activity, request) =>
                    {
                        string reqContent = "request is empty";
                        if(request.Content is not null)
                            reqContent = await request.Content.ReadAsStringAsync();

                        activity.SetTag("http.request.body", reqContent);
                    };
                    httpOptions.EnrichWithHttpResponseMessage = async (activity, response) =>
                    {
                        if(response.Content is not null)
                            activity.SetTag("http.response.body", await response.Content.ReadAsStringAsync());
                    };
                });

                options.AddRedisInstrumentation(redisOptions =>
                {
                    redisOptions.SetVerboseDatabaseStatements = true;
                });
                options.AddConsoleExporter();
                options.AddOtlpExporter();
            });
        }
    }
}
