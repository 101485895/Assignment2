using assgnment.Models;
using Microsoft.EntityFrameworkCore;

namespace assgnment.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Deleting category will delete all products
        modelBuilder.Entity<Category>()
            .HasMany(c => c.products)
            .WithOne(p => p.category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Category>().HasData(
            new Category { categoryId = 1, categoryName = "Electronics", categoryDescription = "TVs, laptops, etc" },
            new Category { categoryId = 2, categoryName = "Clothes", categoryDescription = "shirts, hoodies, pants" },
            new Category { categoryId = 3, categoryName = "Food", categoryDescription = "chips, chocolate etc" }
        );
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders);
    }
}