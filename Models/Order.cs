using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        // 🌟 ĐIỀU CHỈNH: Sử dụng ICollection<OrderLine> thay vì CartLine
        [BindNever]
        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();

        // ---------------- THÔNG TIN KHÁCH HÀNG ----------------

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập dòng địa chỉ thứ nhất")]
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thành phố")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tỉnh/bang")]
        public string? State { get; set; }
        public string? Zip { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên quốc gia")]
        public string? Country { get; set; }

        // ---------------- THÔNG TIN KHÁC ----------------

        // Ghi lại ý muốn gói quà (dữ liệu đơn giản)
        public bool GiftWrap { get; set; } 

        // [BindNever]: Ngăn chặn việc gửi dữ liệu này từ form người dùng
        [BindNever]
        public bool Shipped { get; set; } 

        // [BindNever]: Ngăn chặn việc gửi dữ liệu này từ form người dùng
        // Thuộc tính để lưu trữ thời điểm đặt hàng (Tùy chọn, nên có)
        [BindNever] 
        public DateTime OrderPlaced { get; set; }
    }
}