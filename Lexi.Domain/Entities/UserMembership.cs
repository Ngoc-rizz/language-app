namespace Lexi.Domain.Entities;

public class UserMembership
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string PlanType { get; set; } = string.Empty;  // "monthly" | "yearly"
    public string Status { get; set; } = "active";          // "active" | "expired" | "cancelled"
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public User? User { get; set; }
}
