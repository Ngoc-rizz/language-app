namespace Lexi.Domain.Entities;

public class UserSettings
{
    public Guid UserId { get; set; }
    public bool NotificationEnabled { get; set; } = true;
    public DateTime UpdatedAt { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}