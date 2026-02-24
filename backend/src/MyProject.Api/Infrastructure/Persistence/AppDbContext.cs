using Microsoft.EntityFrameworkCore;
using MyProject.Api.Domain;

namespace MyProject.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.ProductName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ProductImage).HasMaxLength(1000);
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.HasOne(e => e.Product)
                  .WithOne()
                  .HasForeignKey<ProductStock>(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Cart)
                  .WithMany(c => c.Items)
                  .HasForeignKey(e => e.CartId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Product)
                  .WithMany()
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        var p1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var p2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var p3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = p1, ProductName = "Wireless Mouse", ProductPrice = 25.99m, ProductImage = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=500" },
            new Product { ProductId = p2, ProductName = "Mechanical Keyboard", ProductPrice = 89.50m, ProductImage = "https://images.unsplash.com/photo-1595225476474-87563907a212?w=500" },
            new Product { ProductId = p3, ProductName = "27-inch 4K Monitor", ProductPrice = 349.00m, ProductImage = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=500" }
        );

        modelBuilder.Entity<ProductStock>().HasData(
            new ProductStock { ProductId = p1, ProductAmount = 100 },
            new ProductStock { ProductId = p2, ProductAmount = 50 },
            new ProductStock { ProductId = p3, ProductAmount = 20 }
        );
    }
}
