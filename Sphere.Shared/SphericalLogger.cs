using Microsoft.Extensions.Hosting;
using Serilog;

namespace Sphere.Shared;

public static class SphericalLogger
{
    public static ILogger SetupLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.Console()
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
