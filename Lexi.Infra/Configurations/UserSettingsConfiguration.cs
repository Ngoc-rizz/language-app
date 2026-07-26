using Lexi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexi.Infra.Configurations;

public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.ToTable("user_settings");

        builder.HasKey(s => s.UserId);

        builder.Property(s => s.UserId)
            .HasColumnName("user_id");

        builder.Property(s => s.NotificationEnabled)
            .HasColumnName("notification_enabled")
            .HasDefaultValue(true);

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()");
    }
}