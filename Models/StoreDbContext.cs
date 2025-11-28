using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>(); // Thêm DbSet cho Order
        // 🌟 THÊM: DbSet cho ProductVariant (BẮT BUỘC cho logic cập nhật tồn kho) 🌟
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>(); 
        
        // 🌟 THÊM: DbSet cho ProductImage (Khuyến nghị cho tính đầy đủ) 🌟
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<OrderLine> OrderLines => Set<OrderLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    
    modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18, 2)"); 
        
}
    }
}