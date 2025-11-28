using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Cần thiết cho Identity
using SportsStore.Areas.Identity.Data; // Cần thiết cho ApplicationUser
using System.Linq;
using System.Threading.Tasks; // Cần thiết cho async/await
using Microsoft.AspNetCore.Authorization; // Vẫn giữ lại nhưng không áp dụng cho GET Checkout

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        // Giữ lại UserManager để dùng khi người dùng ĐÃ đăng nhập
        private readonly UserManager<ApplicationUser> userManager; 

        // Constructor CẬP NHẬT: Thêm UserManager
        public OrderController(IOrderRepository repoService, Cart cartService, UserManager<ApplicationUser> userMgr)
        {
            repository = repoService;
            cart = cartService;
            userManager = userMgr; 
        }
        
        // --- Phương thức GET: Hiển thị form Checkout ---
        // 🌟 CẬP NHẬT: ĐÃ XÓA [Authorize] để cho phép Khách vãng lai (Guest) truy cập 🌟
        [HttpGet]
        public async Task<ViewResult> Checkout()
        {
            var order = new Order();
            
            // 🌟 LOGIC CẬP NHẬT: CHỈ TỰ ĐỘNG ĐIỀN NẾU NGƯỜI DÙNG ĐÃ ĐĂNG NHẬP 🌟
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser? user = await userManager.GetUserAsync(User);

                if (user != null)
                {
                    // Ánh xạ các trường từ ApplicationUser sang Order
                    // Tên người nhận (Name) -> FullName
                    order.Name = user.FullName;

                    // Email -> Email
                    order.Email = user.Email;

                    // Số điện thoại (PhoneNumber) -> PhoneNumber
                    order.PhoneNumber = user.PhoneNumber; 
                    
                    // Dòng 1 (Line 1) -> Address
                    order.Line1 = user.Address;

                    // Các trường địa chỉ còn lại để trống theo yêu cầu
                }
            } else {
                 // Nếu không đăng nhập, trả về Order trống để người dùng tự nhập
            }
            
            return View(order);
        }

        // --- Phương thức POST: Xử lý khi Submit form ---
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Xin lỗi, giỏ hàng của bạn đang trống!");
            }

            if (ModelState.IsValid)
            {
                // 🌟 LOGIC CẬP NHẬT: Gán AppUserId chỉ khi ĐÃ đăng nhập 🌟
                if (User.Identity.IsAuthenticated)
                {
                    // Lấy ID của người dùng hiện tại và gán vào khóa ngoại
                    order.AppUserId = userManager.GetUserId(User);
                } else {
                    // Nếu là khách vãng lai, AppUserId sẽ là NULL (hoặc 0)
                    order.AppUserId = null; 
                }
                
                // 1. Gán OrderLine bằng cách chuyển đổi từ CartLine (Giữ nguyên logic cũ)
                order.Lines = cart.Lines
                    .Select(cl => new OrderLine
                    {
                        ProductID = (long)cl.Product.ProductID,
                        ProductName = cl.Product.Name,
                        ProductVariantID = cl.ProductVariantID,
                        ProductSize = cl.Product.Variants
                                                 ?.FirstOrDefault(v => v.ProductVariantID == cl.ProductVariantID)?.Size 
                                                 ?? "N/A",
                        Price = cl.Product.Price,
                        Quantity = cl.Quantity,
                        
                    }).ToList();

                // 2. Cập nhật thời điểm đặt hàng
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