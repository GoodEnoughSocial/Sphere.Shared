using Consul;

using Flurl;

namespace Sphere.Shared;

public static class Services
{
    // TODO: make data driven, or dynamic...
    public static readonly ServiceDefinition Administration = new("Administration", "https", "localhost", 5002);
    public static readonly ServiceDefinition Auth = new("Auth", "https", "localhost", 5004);
    public static readonly ServiceDefinition SphereWebTestApp = new("WebTestApp", "https", "localhost", 5013);
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
    };
}
