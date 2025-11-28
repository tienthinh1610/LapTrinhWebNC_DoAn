using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using SportsStore.Areas.Identity.Data;
using SportsStore.Services; // 👈 THÊM DÒNG NÀY: Để nhận diện lớp EmailSender
using Microsoft.AspNetCore.Identity.UI.Services; // 👈 THÊM DÒNG NÀY: Để nhận diện interface IEmailSender

var builder = WebApplication.CreateBuilder(args);

// Dòng này đã được xử lý ở dưới với AddJsonOptions, nên có thể xóa ở đây nếu trùng lặp.
// Tuy nhiên, nếu bạn muốn giữ nó ở đầu để đảm bảo Controller/View hoạt động sớm, hãy giữ lại.
// builder.Services.AddControllersWithViews(); 

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
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppIdentityDbContext>() // ĐÃ ĐỔI TÊN THÀNH AppIdentityDbContext
    .AddDefaultTokenProviders(); 

// 🎯 DÒNG QUAN TRỌNG NHẤT: ĐĂNG KÝ DỊCH VỤ GỬI EMAIL GIẢ 🎯
// Giải quyết lỗi System.InvalidOperationException: Unable to resolve service for type 'IEmailSender'
builder.Services.AddTransient<IEmailSender, EmailSender>(); 



builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServerSideBlazor();

// Cấu hình Controller và JSON (Đã hợp nhất với khai báo đầu tiên, nhưng giữ lại ở đây để chứa AddJsonOptions)
// Nếu dòng AddControllersWithViews() đầu tiên bị xóa, dòng này sẽ được dùng.
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

app.UseRouting();
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