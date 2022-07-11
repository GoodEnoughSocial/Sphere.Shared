namespace Sphere.Shared.Models;

[Serializable]
public record UserProfileState(Description Description, string? AvatarMediaUrl, string? HeaderMediaUrl);

[Serializable]
public record Description(string? Bio, string? Pronouns, string? Website, string? Location);
