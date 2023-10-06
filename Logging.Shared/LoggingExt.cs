using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;

namespace Logging.Shared
{
    public static class LoggingExt
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging => (builderContext, loggerConfiguration) =>
        {
            var enviroment = builderContext.HostingEnvironment;
            loggerConfiguration
            .ReadFrom.Configuration(builderContext.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Env", enviroment.EnvironmentName)
            .Enrich.WithProperty("AppName", enviroment.ApplicationName);

            var elasticsearchBaseUrl = builderContext.Configuration.GetSection("Elasticsearch")["BaseUrl"];
            var elasticsearchUsername = builderContext.Configuration.GetSection("Elasticsearch")["Username"];
            var elasticsearchPassword = builderContext.Configuration.GetSection("Elasticsearch")["Password"];
            var elasticsearchIndexName = builderContext.Configuration.GetSection("Elasticsearch")["IndexName"];

            loggerConfiguration.WriteTo.Elasticsearch(new(new Uri(elasticsearchBaseUrl!))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                IndexFormat = $"{elasticsearchIndexName}-{enviroment.EnvironmentName}-logs-"+"{0:yyy.MM.dd}",
                ModifyConnectionSettings = x => x.BasicAuthentication(elasticsearchUsername, elasticsearchPassword),
                CustomFormatter = new ElasticsearchJsonFormatter()
            });
        };
    }
}