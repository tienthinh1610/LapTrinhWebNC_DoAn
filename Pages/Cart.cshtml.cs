using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;
        public Cart Cart { get; set; }

        public CartModel(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService; // Sử dụng service thay vì session trực tiếp
        }

        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            
            // ----------------------------------------------------
            // 🌟 LOGIC TẢI LẠI DỮ LIỆU ĐỂ HIỂN THỊ ẢNH VÀ SIZE
            // ----------------------------------------------------
            foreach (var line in Cart.Lines)
            {
                // Truy vấn lại Product từ DB, và BẮT BUỘC Include các collection
                line.Product = repository.Products
                    .Include(p => p.Images)    // ⬅️ Tải Ảnh
                    .Include(p => p.Variants)  // ⬅️ Tải Variants/Size
                    .FirstOrDefault(p => p.ProductID == line.Product.ProductID);
            }
        }

        public IActionResult OnPost(long productId, string returnUrl, int? selectedVariantId)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            
            if (product != null)
            {
                // 2. Gọi AddItem với tham số ProductVariantID đã nhận được
                // Cẩn thận đừng quên số 1 (Quantity) nhé!
                Cart.AddItem(product, 1, selectedVariantId); 
            }
            
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl =>
                cl.Product.ProductID == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}