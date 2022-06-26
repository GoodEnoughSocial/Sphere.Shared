using Microsoft.Extensions.Hosting;
using Serilog;

namespace Sphere.Shared;

public static class SphericalLogger
{
    public static void SetupLogger(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
    {
        configuration = GetLoggerConfiguration()
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services);
    }

    public static ILogger StartupLogger(string serviceName)
    {
        var logger = GetLoggerConfiguration().CreateBootstrapLogger();

        Log.ForContext(LogContexts.ApplicationName, serviceName);

        Log.Information("Starting up: {Name}", serviceName);

        return logger;
    }

    public static LoggerConfiguration GetLoggerConfiguration()
    {
        return new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext} {Message:lj}{NewLine}{Exception}{NewLine}")
            //            .WriteTo.Kafka(
            //                new JsonFormatter(),
            //                new KafkaOptions(new List<string> { "localhost:9094" }, "Logs-sphere"))
            .Enrich.FromLogContext();
    }
}

public static class LogContexts
{
    public const string ApplicationName = nameof(ApplicationName);
}
