using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL;

public sealed class RepositoryContext : DbContext
{
    public RepositoryContext()
    {
        Database.EnsureCreated();
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();


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
        modelBuilder.Entity<Cart>().HasData(DataInitializer.InitializeCarts());
    }
}