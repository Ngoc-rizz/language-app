using Lexi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexi.Infra.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id");
        builder.Property(u => u.Email)
       .HasColumnName("email")
       .HasMaxLength(255)
       .IsRequired();
        builder.Property(u => u.PasswordHash)
             .HasColumnName("password_hash");

        builder.Property(u => u.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(255);

        builder.Property(u => u.AvatarUrl)
            .HasColumnName("avatar_url");

        builder.Property(u => u.LoginType)
            .HasColumnName("login_type")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.GoogleId)
            .HasColumnName("google_id")
            .HasMaxLength(255);

        builder.Property(u => u.EmailVerified)
            .HasColumnName("email_verified")
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()");
            
        // Unique index - email luôn unique
        builder.HasIndex(u => u.Email).IsUnique();

        // Unique index - google_id unique nhưng cho phép nhiều null (Postgres mặc định filter được)
        builder.HasIndex(u => u.GoogleId).IsUnique();

        // 1-1 với UserSettings
        builder.HasOne(u => u.Settings)
            .WithOne(s => s.User)
            .HasForeignKey<UserSettings>(s => s.UserId);

        // 1-n với UserMembership
        builder.HasMany(u => u.Memberships)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId);
    }
}