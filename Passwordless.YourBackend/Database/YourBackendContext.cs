using Microsoft.EntityFrameworkCore;
using Passwordless.YourBackend.Database.Models;

namespace Passwordless.YourBackend.Database;

public class YourBackendContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=localhost");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Username).IsRequired().HasMaxLength(32);
            b.Property(x => x.FirstName).IsRequired().HasMaxLength(64);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(64);
        });
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Role>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(32);
        });
        modelBuilder.Entity<Role>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<User>()
            .HasMany(e => e.Roles)
            .WithMany(e => e.Users);
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
}