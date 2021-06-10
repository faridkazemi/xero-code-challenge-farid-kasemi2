using Microsoft.EntityFrameworkCore;
using RefactorThis.DB.Entity;

namespace DB
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductOption> ProductOption { get; set; }
    }
}