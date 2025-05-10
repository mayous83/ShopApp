using Microsoft.EntityFrameworkCore;
using ShopApp.Shared.Library.Database;

namespace ShopApp.Users.Service.Database;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options){ }
    
    public DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configuration to all entities inheriting BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Ensure Id is the PK
                modelBuilder.Entity(entityType.ClrType)
                    .HasKey(nameof(BaseEntity.Id));

                // Ensure ExternalId is unique
                modelBuilder.Entity(entityType.ClrType)
                    .HasIndex(nameof(BaseEntity.EntityId))
                    .IsUnique();
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}