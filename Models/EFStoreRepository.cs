using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext context;

        public EFStoreRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
        // 🌟 CÀI ĐẶT MỚI: Tải sản phẩm kèm theo Variants và Images
        public Product? GetProductWithDetails(long id)
        {
            return context.Products
                .Include(p => p.Variants) // Tải các Biến thể (Size, Color, Quantity)
                .Include(p => p.Images) // Tải các Hình ảnh
                .FirstOrDefault(p => p.ProductID == id);
        }

        public void CreateProduct(Product p)
{
    // EF Core tự động thêm Variants/Images nếu chúng có ID = 0
    context.Add(p);
    context.SaveChanges();
}

public void SaveProduct(Product p)
{
    // 🌟 CÀI ĐẶT TỐI ƯU: Đảm bảo EF Core theo dõi và xử lý các đối tượng con
    
    // Đánh dấu sản phẩm là đã sửa đổi
    context.Attach(p); 
    context.Entry(p).State = EntityState.Modified;

    // 1. Xử lý Variants (Nếu có Variants mới/sửa/xóa)
    if (p.Variants != null)
    {
        foreach (var v in p.Variants)
        {
            if (v.ProductVariantID == 0)
            {
                // Biến thể mới
                context.Entry(v).State = EntityState.Added;
            }
            else
            {
                // Biến thể đã tồn tại, đánh dấu là Modified
                context.Entry(v).State = EntityState.Modified;
            }
        }
    }
    // Cần có logic để xóa Variants bị loại bỏ (Nếu bạn dùng List.Remove trong Editor)
    
    // 2. Xử lý Images (Tương tự Variants)
    if (p.Images != null)
    {
        foreach (var img in p.Images)
        {
            if (img.ProductImageID == 0)
            {
                context.Entry(img).State = EntityState.Added;
            }
            else
            {
                context.Entry(img).State = EntityState.Modified;
            }
        }
    }

    context.SaveChanges();
}

public void DeleteProduct(Product p)
{
    // EF Core thường sẽ tự động xử lý xóa các đối tượng con (Cascade Delete)
    context.Remove(p);
    context.SaveChanges();
}
    }
}