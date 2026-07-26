namespace Lexi.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public string LoginType { get; set; } = "email";
    public string? GoogleId { get; set; }
    public bool EmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public UserSettings Settings { get; set; } = new(); // Một User có duy nhất 1 UserSettings
    public ICollection<UserMembership> Memberships { get; set; } = new List<UserMembership>(); //Một collection chứa nhiều UserMembership
}