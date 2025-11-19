using System.Text.Json.Serialization; // Cần thêm using này
namespace SportsStore.Models
{
    // Lớp này dùng để lưu trữ thông tin của MỘT file ảnh
    public class ProductImage
    {
        public int ProductImageID { get; set; }
        
        // Đường dẫn thực tế của file ảnh (lưu URL)
        public string ImageUrl { get; set; } = string.Empty; 
        
        // Thuộc tính này giúp xác định đây là ảnh chính hay ảnh phụ
        public bool IsMainImage { get; set; } = false; 
        
        // Thuộc tính để sắp xếp thứ tự hiển thị (Ví dụ: 1, 2, 3...)
        public int DisplayOrder { get; set; }
        
        // --- KHÓA NGOẠI: Liên kết với Sản phẩm Gốc ---
        // (Giả sử bạn muốn một sản phẩm chung có nhiều ảnh, không phải biến thể)
        // 🌟 KHẮC PHỤC: Bỏ qua tham chiếu ngược này khi Serialization
        
        public long? ProductID { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }
        
        // HOẶC
        
        // --- KHÓA NGOẠI: Liên kết với Biến thể Sản phẩm ---
        // (Nếu bạn muốn một biến thể (M/Đỏ) có nhiều ảnh)
        // public int? ProductVariantID { get; set; }
        // public virtual ProductVariant? Variant { get; set; }
    }
}