using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Product
    {
        public long? ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        
        public string Category { get; set; } = string.Empty;

        // ---- THAY THẾ MainImageUrl bằng một Collection ----
        // Một Sản phẩm sẽ có nhiều ảnh (One-to-Many Relationship)
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}