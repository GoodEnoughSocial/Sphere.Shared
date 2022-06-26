using System.Net.Sockets;
using Consul;
using Flurl;
using Serilog;

namespace Sphere.Shared;

public static class Services
{
    public static string Current = string.Empty;

    // Helper strings to make common services easily available.
    public const string Accounts = nameof(Accounts);
    public const string Administration = nameof(Administration);
    public const string ApiGateway = nameof(ApiGateway);
    public const string Auth = nameof(Auth);
    public const string Branding = nameof(Branding);
    public const string Health = nameof(Health);
    public const string Media = nameof(Media);
    public const string Messages = nameof(Messages);
    public const string Profiles = nameof(Profiles);
    public const string Relationships = nameof(Relationships);
    public const string Spheres = nameof(Sphere);
    public const string Timeline = nameof(Timeline);
    public const string SphereWebTestApp = nameof(SphereWebTestApp);
    public const string Server = nameof(Server);

    private static ConsulClient? client = null;
    public static async Task<WriteResult> RegisterService(AgentServiceRegistration registration, CancellationToken token = default)
    {
        if (client is null)
        {
            client = new ConsulClient();
        }

        return await client.Agent.ServiceRegister(registration, token);
    }

    public static async Task<WriteResult> UnregisterService(AgentServiceRegistration? registration, CancellationToken token = default)
    {
        try
        {
            return client is null || registration is null
                ? throw new Exception($"{registration?.Name ?? "Unnamed"} service was never registered, or client was set to null")
                : await client.Agent.ServiceDeregister(registration.ID, token);
        }
        catch (SocketException sockEx)
        {
            Log.Fatal(sockEx, "Error while unregistering service: {Registration}", registration);
            throw;
        }

    }
}

[Serializable]
public record ServiceDefinition
{
    public ServiceDefinition()
    {
    }

    public ServiceDefinition(string name, string scheme, string host, int port)
    {
        this.Name = name;
        this.Scheme = scheme;
        this.Host = host;
        this.Port = port;
    }

    public string Name { get; init; } = string.Empty;
    public string Scheme { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; } = 0;

    public string Address => $"{Scheme}://{Host}:{Port}";
    public string Combine(params string[] parts) => Url.Combine(parts.Prepend(Address).ToArray());

    public AgentServiceRegistration GetServiceRegistration()
    => new()
    {
        ID = Guid.NewGuid().ToString(),
        Address = Host,
        Port = Port,
        Name = Name,
        Check = new()
        {
            HTTP = $"https://host.docker.internal:{Port}/health",
            Interval = TimeSpan.FromSeconds(1),
            TLSSkipVerify = true,
        },
    };
}
