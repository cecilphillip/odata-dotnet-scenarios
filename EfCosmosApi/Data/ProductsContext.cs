using Microsoft.EntityFrameworkCore;

namespace EfCosmosApi.Data
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasPartitionKey(p => p.ID)
                //.HasNoDiscriminator()
                .ToContainer("catalog");

            modelBuilder.Entity<Product>()
                .Property(p => p.ID).ToJsonProperty("productId");

            modelBuilder.Entity<Product>()
                .Property(p => p.Name).ToJsonProperty("name");

            modelBuilder.Entity<Product>()
                .Property(p => p.Category).ToJsonProperty("category");

            modelBuilder.Entity<Product>()
                .Property(p => p.BarCode).ToJsonProperty("barcode");

            modelBuilder.Entity<Product>()
                .Property(p => p.Quantity).ToJsonProperty("quantity");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price).ToJsonProperty("price");
        }
    }
}