using Microsoft.EntityFrameworkCore;
using ShopApp.Orders.Service.Database.Migrations;
using ShopApp.Shared.Library.Database;

namespace ShopApp.Orders.Service.Database;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options){ }
    
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    
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
        
        modelBuilder.Entity<Order>()
            .HasMany<OrderItem>(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}