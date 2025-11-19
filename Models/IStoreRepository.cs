namespace SportsStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
        
        // 🌟 MỚI: Phương thức tải sản phẩm kèm theo chi tiết
        Product? GetProductWithDetails(long id); 
        
        void SaveProduct(Product p);
        void CreateProduct(Product p);
        void DeleteProduct(Product p);
    }
}