using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore; // Cần thêm namespace này cho .Include()

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository repository;
        public int PageSize = 4;

        public HomeController(IStoreRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string? category, int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = repository.Products
                    // ----------------------------------------------------------------------------------
                    // 🌟 BỔ SUNG: DÙNG .Include() ĐỂ TẢI KÈM DỮ LIỆU LIÊN KẾT (EAGER LOADING)
                    // Đây là thay đổi BẮT BUỘC để Model.Images và Model.Variants không bị NULL trong View.
                    .Include(p => p.Images)    // Tải kèm Collection Images
                    .Include(p => p.Variants)  // Tải kèm Collection Variants
                    // ----------------------------------------------------------------------------------
                    .Where(p => category == null || p.Category == category) // Lọc theo danh mục
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null 
                        ? repository.Products.Count() 
                        : repository.Products.Where(e => e.Category == category).Count() // Đếm theo danh mục
                },
                CurrentCategory = category // Truyền danh mục hiện tại
            });  
            // Hàm này sẽ được gọi khi người dùng truy cập /Home/Details/{id}
        public ViewResult Details(long id)
        {
            // Truy vấn sản phẩm:
            var product = repository.Products
                // 🌟 INCLUDE các bảng liên quan để hiển thị Ảnh và Size
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                // Xử lý trường hợp ID không tồn tại trong Database
                // Trả về một View lỗi, ví dụ: NotFound.cshtml
                // Bạn cần tự tạo View này.
                Response.StatusCode = 404;
                return View("NotFound"); 
            }

            // Trả về View Details.cshtml và truyền đối tượng Product
            return View(product);
        }
    }
}