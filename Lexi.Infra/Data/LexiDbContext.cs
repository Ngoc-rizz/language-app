using Lexi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lexi.Infra.Data;

public class LexiDbContext : DbContext
{
    public LexiDbContext(DbContextOptions<LexiDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<UserMembership> UserMemberships => Set<UserMembership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tự động apply toàn bộ IEntityTypeConfiguration trong assembly này
        // (khi thêm entity mới - Word, Folder, Study... chỉ cần tạo Configuration
        // tương ứng, không cần sửa dòng nào ở đây)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LexiDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}