using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Areas.Identity.Data;
using System.Threading.Tasks;

// View Component này sẽ chứa logic kiểm tra trạng thái đăng nhập và lấy thông tin FullName
// để tránh lỗi Deadlock/Rendering khi gọi bất đồng bộ trong Partial View.
namespace SportsStore.Components
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        // Dependency Injection qua constructor
        public LoginStatusViewComponent(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Phương thức InvokeAsync chứa logic bất đồng bộ
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Kiểm tra trạng thái đăng nhập
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                // Lấy đối tượng ApplicationUser đầy đủ (bao gồm FullName)
                var user = await _userManager.GetUserAsync(HttpContext.User);
                
                // Trả về view Default.cshtml, truyền đối tượng user
                return View(user); 
            }
            
            // Nếu chưa đăng nhập, trả về view Default.cshtml với Model là null
            return View(null);
        }
    }
}