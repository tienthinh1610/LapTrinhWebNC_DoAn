using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Cần cho các thuộc tính đặc biệt
using Microsoft.AspNetCore.Mvc.ModelBinding;
// Đảm bảo bạn có lớp ApplicationUser trong dự án (hoặc sử dụng fully qualified name)

namespace SportsStore.Models
{
    public class Order
    {
        // ---------------- THÔNG TIN ĐƠN HÀNG CƠ BẢN (PRIMARY KEY) ----------------
        
        [BindNever] // Ngăn chặn dữ liệu gửi từ form người dùng
        public int OrderID { get; set; }

        // ---------------- MỐI QUAN HỆ (RELATIONSHIPS) ----------------

        // 1. Order Lines (Chi tiết đơn hàng) - Quan hệ 1-N (Order - OrderLine)
        [BindNever]
        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
        
        // 2. Liên kết với người dùng đã đăng ký (ApplicationUser)
        [BindNever] 
        public string? AppUserId { get; set; } // Khóa ngoại (Foreign Key)
        
        // 🌟 Navigation Property cho ApplicationUser (Tùy chọn, nhưng nên có)
        // [BindNever] 
        // public ApplicationUser? AppUser { get; set; } // Giả sử ApplicationUser nằm trong Models

        // ---------------- THÔNG TIN KHÁCH HÀNG ----------------

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string? Email { get; set; } // Thường dùng để gửi xác nhận đơn hàng

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15)]
        public string? PhoneNumber { get; set; } // Cần thiết cho việc giao hàng

        // ---------------- THÔNG TIN GIAO HÀNG ----------------

        [Required(ErrorMessage = "Vui lòng nhập dòng địa chỉ thứ nhất")]
        [StringLength(100)]
        public string? Line1 { get; set; }
        
        [StringLength(100)]
        public string? Line2 { get; set; }
        
        [StringLength(100)]
        public string? Line3 { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thành phố")]
        [StringLength(50)]
        public string? City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tỉnh/bang")]
        [StringLength(50)]
        public string? State { get; set; }
        
        [StringLength(20)]
        public string? Zip { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên quốc gia")]
        [StringLength(50)]
        public string? Country { get; set; }

        // ---------------- THÔNG TIN ĐẶT HÀNG/THANH TOÁN ----------------

        // Ghi lại ý muốn gói quà
        public bool GiftWrap { get; set; } 

        // [BindNever]: Ngăn chặn việc gửi dữ liệu này từ form người dùng
        [BindNever]
        public bool Shipped { get; set; } // Trạng thái đã giao hàng hay chưa

        // Thuộc tính để lưu trữ thời điểm đặt hàng
        [BindNever] 
        public DateTime OrderPlaced { get; set; } = DateTime.Now; // Set giá trị mặc định

        // 🌟 Thuộc tính mới: Tổng giá trị đơn hàng (Tính toán từ Lines)
        // [NotMapped]
        // public decimal OrderTotal => Lines.Sum(l => l.Quantity * l.Product.Price); 
        // Lưu ý: Cần đảm bảo Lines được load và OrderLine có giá Price.
    }
}