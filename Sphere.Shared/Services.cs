using Consul;
using Flurl;

namespace Sphere.Shared;

public static class Services
{
    // TODO: make data driven, or dynamic...
    public static readonly ServiceDefinition Accounts = new(nameof(Accounts) + "v1", "https", "localhost", 5001);
    public static readonly ServiceDefinition Administration = new(nameof(Administration) + "v1", "https", "localhost", 5002);
    public static readonly ServiceDefinition ApiGateway = new(nameof(ApiGateway) + "v1", "https", "localhost", 5003);
    public static readonly ServiceDefinition Auth = new(nameof(Auth) + "v1", "https", "localhost", 5004);
    public static readonly ServiceDefinition Branding = new(nameof(Branding) + "v1", "https", "localhost", 5005);
    public static readonly ServiceDefinition Health = new(nameof(Health) + "v1", "https", "localhost", 5006);
    public static readonly ServiceDefinition Media = new(nameof(Media) + "v1", "https", "localhost", 5007);
    public static readonly ServiceDefinition Messages = new(nameof(Messages) + "v1", "https", "localhost", 5008);
    public static readonly ServiceDefinition Profiles = new(nameof(Profiles) + "v1", "https", "localhost", 5009);
    public static readonly ServiceDefinition Relationships = new(nameof(Relationships) + "v1", "https", "localhost", 5010);
    public static readonly ServiceDefinition Spheres = new(nameof(Spheres) + "v1", "https", "localhost", 5011);
    public static readonly ServiceDefinition Timeline = new(nameof(Timeline) + "v1", "https", "localhost", 5012);
    public static readonly ServiceDefinition SphereWebTestApp = new(nameof(SphereWebTestApp) + "v1", "https", "localhost", 5013);

    private static ConsulClient? client = null;
    public static async Task<WriteResult> RegisterService(AgentServiceRegistration registration, CancellationToken token = default)
    {
        if (client is null)
        {
            client = new ConsulClient();
        }

        return await client.Agent.ServiceRegister(registration, token);
    }

    public static async Task<WriteResult> UnregisterService(AgentServiceRegistration registration, CancellationToken token = default)
    {
        return client is null
            ? throw new Exception($"{registration.Name} service was never registered, or client was set to null")
            : await client.Agent.ServiceDeregister(registration.ID, token);
    }
}

public record ServiceDefinition(string Name, string Scheme, string Host, int Port)
{
    public string Address = $"{Scheme}://{Host}:{Port}";
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
            HTTP = $"{Address}/health",
            Interval = TimeSpan.FromSeconds(1),
        },
    };
}
