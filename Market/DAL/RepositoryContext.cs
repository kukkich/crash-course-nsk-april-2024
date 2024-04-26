using Market.Models;
using Market.Models.Orders;
using Market.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL;

internal sealed class RepositoryContext : DbContext
{
    public RepositoryContext()
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();
    public DbSet<OrderedProductItem> OrderedProductItems { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(DataInitializer.InitializeProducts());
        
        modelBuilder.Entity<Cart>().HasKey(c => c.CustomerId);
        modelBuilder.Entity<Cart>().HasData(DataInitializer.InitializeCarts());
    }
}