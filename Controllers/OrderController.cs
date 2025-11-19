using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore; // Cần thiết nếu bạn muốn Eager Load dữ liệu Product

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }
        

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Xin lỗi, giỏ hàng của bạn đang trống!");
            }

            if (ModelState.IsValid)
            {
                // 1. Gán OrderLine bằng cách chuyển đổi từ CartLine
                order.Lines = cart.Lines
                    .Select(cl => new OrderLine
                    {
                        // 🌟 Sao chép thông tin Sản phẩm Gốc
                        ProductID = (long)cl.Product.ProductID,
                        ProductName = cl.Product.Name,
                        
                        // 🌟 Sao chép thông tin Biến thể (Size)
                        ProductVariantID = cl.ProductVariantID,
                        ProductSize = cl.Product.Variants
                                        ?.FirstOrDefault(v => v.ProductVariantID == cl.ProductVariantID)?.Size 
                                        ?? "N/A", // Tìm Size dựa trên VariantID
                                        
                        // 🌟 Sao chép Giá và Số lượng tại thời điểm đặt hàng
                        Price = cl.Product.Price, // Giá hiện tại của sản phẩm
                        Quantity = cl.Quantity,
                        
                    }).ToList(); // Chuyển đổi thành List<OrderLine>

                // 2. Cập nhật thời điểm đặt hàng (Tùy chọn, nhưng nên có)
                order.OrderPlaced = DateTime.Now; 
                
                // 3. Lưu đơn hàng
                repository.SaveOrder(order);
                
                // 4. Xóa giỏ hàng
                cart.Clear();
                
                // 5. Chuyển hướng đến trang xác nhận (Completed)
                return RedirectToPage("/Completed", new { orderId = order.OrderID });
            }
            else
            {
                // Nếu Validation thất bại, trả về View với dữ liệu đã nhập
                return View();
            }
        }
    }
}