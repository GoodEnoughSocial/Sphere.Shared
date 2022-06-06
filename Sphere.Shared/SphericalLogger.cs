using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Kafka;
using Serilog.Sinks.Kafka.Options;

namespace Sphere.Shared;

public static class SphericalLogger
{
    public static ILogger SetupLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.Kafka(
                new JsonFormatter(),
                new KafkaOptions(new List<string> { "localhost:9094" }, "Logs-sphere"))
            .CreateBootstrapLogger();
    }

    public static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration configuration)
    {
        configuration
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(context.Configuration);
    }
}
