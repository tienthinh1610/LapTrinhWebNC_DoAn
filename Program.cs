using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using SportsStore.Areas.Identity.Data;
// using SportsStore.Data; // Thêm namespace nếu ApplicationDbContext nằm ở đây

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // Chỉ khai báo một lần

// Khai báo StoreDbContext (Quản lý sản phẩm)
builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

// 🌟 PHẦN SỬA LỖI QUAN TRỌNG: Cấu hình Identity 🌟
// 1. Khai báo DbContext của Identity (ĐÃ ĐỔI TÊN THÀNH AppIdentityDbContext)
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:IdentityConnection"]));

// 2. Cấu hình Identity với ApplicationUser và sử dụng AppIdentityDbContext
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppIdentityDbContext>() // ĐÃ ĐỔI TÊN THÀNH AppIdentityDbContext
    .AddDefaultTokenProviders(); // Quan trọng để hỗ trợ các chức năng như reset password

// Xóa các dòng cấu hình Identity bị trùng lặp/xung đột trước đó:
// - Bỏ `builder.Services.AddDefaultIdentity<ApplicationUser>(...).AddEntityFrameworkStores<ApplicationDbContext>();`
// - Bỏ `builder.Services.AddDbContext<AppIdentityDbContext>(...)` // Dòng này bị xóa hoặc thay thế
// - Bỏ `builder.Services.AddIdentity<IdentityUser, IdentityRole>()...`

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServerSideBlazor();

// Cấu hình Controller và JSON (Đã hợp nhất với khai báo đầu tiên, nhưng giữ lại ở đây để chứa AddJsonOptions)
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Giải quyết lỗi JSON Cycle khi Serialization
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 64; 
    });

var app = builder.Build();

if (app.Environment.IsProduction()) {
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();
app.UseSession();

// Thêm Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Cấu hình Routing
app.MapControllerRoute("catpage", "{category}/Page{productPage:int}",
    new { Controller = "Home", action = "Index" });
app.MapControllerRoute("page", "Page{productPage:int}",
    new { Controller = "Home", action = "Index", productPage = 1 });
app.MapControllerRoute("category", "{category}",
    new { Controller = "Home", action = "Index", productPage = 1 });
app.MapControllerRoute("pagination", "Products/Page{productPage}",
    new { Controller = "Home", action = "Index", productPage = 1 });
app.MapDefaultControllerRoute();
app.MapRazorPages(); // Quan trọng để các trang Identity hoạt động
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

SeedData.EnsurePopulated(app);
// IdentitySeedData.EnsurePopulated(app); // Hãy chạy Migration trước khi bật Seed Data

app.Run();