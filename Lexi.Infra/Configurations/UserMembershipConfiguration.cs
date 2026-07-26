using Lexi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexi.Infra.Configurations;

public class UserMembershipConfiguration : IEntityTypeConfiguration<UserMembership>
{
    public void Configure(EntityTypeBuilder<UserMembership> builder)
    {
        builder.ToTable("user_memberships");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.UserId)
            .HasColumnName("user_id");

        builder.Property(m => m.PlanType)
            .HasColumnName("plan_type")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("active");

        builder.Property(m => m.StartAt)
            .HasColumnName("start_at");

        builder.Property(m => m.EndAt)
            .HasColumnName("end_at");

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        // Index để query nhanh "membership đang active của user"
        builder.HasIndex(m => new { m.UserId, m.Status });
    }
}