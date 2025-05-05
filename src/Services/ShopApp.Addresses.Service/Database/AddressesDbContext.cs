using Microsoft.EntityFrameworkCore;
using ShopApp.Shared.Library;

namespace ShopApp.Addresses.Service.Database;

public sealed class AddressesDbContext : DbContext
{
    public AddressesDbContext(DbContextOptions<AddressesDbContext> options) : base(options){ }
    
    public DbSet<Address> Addresses => Set<Address>();
    
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