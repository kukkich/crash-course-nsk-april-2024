using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL;

internal sealed class RepositoryContext : DbContext
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
        
        modelBuilder.Entity<Cart>().HasKey(c => c.CustomerId);
        modelBuilder.Entity<Cart>().Property(c => c.ProductIds).HasColumnType("TEXT")
            .HasConversion(
                ids => string.Join(';', ids), 
                s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());
        modelBuilder.Entity<Cart>().HasData(DataInitializer.InitializeCarts());
    }
}