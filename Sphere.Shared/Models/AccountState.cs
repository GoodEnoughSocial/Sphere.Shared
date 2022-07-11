using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Sphere.Shared.Models;

[Serializable]
public class AccountState
{
    [EmailAddress, Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string AccountDisplayName { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    public long AccountId { get; set; }
    public CreatedWith CreatedWith { get; set; } = CreatedWith.Unknown;
    public DateTime CreatedAt { get; set; }
    public string? TimeZone { get; set; }
    public DateOnly? BirthDate { get; set; }
    public IPAddress? CreationIP { get; set; }
}

[Serializable]
public enum CreatedWith
{
    Unknown = 0,
    Web,
    Mobile,
}
