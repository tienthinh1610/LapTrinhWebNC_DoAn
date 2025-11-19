// Trong file OrderLine.cs (Hoặc CartLine nếu bạn quyết định đổi tên và tái sử dụng)

using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class OrderLine
    {
        public int OrderLineID { get; set; }
        
        // --- Dữ liệu chi tiết sản phẩm ---
        
        // 🌟 LƯU ID GỐC: ID sản phẩm và ID biến thể (Size)
        public long ProductID { get; set; } 
        public int? ProductVariantID { get; set; } 
        
        // 🌟 LƯU TÊN và GIÁ: Để ghi lại tại thời điểm đặt hàng (tính bất biến)
        public string ProductName { get; set; } = string.Empty;
        public string ProductSize { get; set; } = string.Empty; // Size đã chọn
        public decimal Price { get; set; } 
        
        public int Quantity { get; set; }

        // --- Tham chiếu ngược ---
        
        [ForeignKey("OrderID")] // Thiết lập khóa ngoại
        public int OrderID { get; set; }
        // [JsonIgnore] // Nếu bạn đã cấu hình xử lý vòng lặp JSON toàn cục, có thể bỏ qua
        public Order Order { get; set; } = new Order(); // Tham chiếu ngược về Order
    }
}