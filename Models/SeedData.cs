using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            StoreDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Products.Any())
            {
                // Danh sách Size cố định để tái sử dụng
                var sizes = new List<string> { "40", "41", "42" };
                const int DefaultQuantity = 3;

                // Sử dụng AddRange để thêm TẤT CẢ các sản phẩm đã được khởi tạo tường minh.
                context.Products.AddRange(
                    
                    // =========================================================================
                    // 👟 SẢN PHẨM 1: Adidas1
                    // =========================================================================
                    new Product
                    {
                        Name = "Adidas Samba",
                        Description = "B75806",
                        Category = "Adidas",
                        Price = 75.50m, // Giá được chỉ định rõ ràng
                        
                        // --- 1. BIẾN THỂ (Variants) ---
                        Variants = new List<ProductVariant>
                        {
                            new ProductVariant { Size = "40", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "41", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "42", Color = "", Quantity = DefaultQuantity }
                        },
                        
                        // --- 2. HÌNH ẢNH (Images) ---
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "/images/samba_B75806(1).jpeg", IsMainImage = true, DisplayOrder = 1 },
                            new ProductImage { ImageUrl = "/images/samba_B75806(2).jpeg", IsMainImage = false, DisplayOrder = 2 },
                            new ProductImage { ImageUrl = "/images/samba_B75806(3).jpeg", IsMainImage = false, DisplayOrder = 3 }
                        }
                    },
                    
                    // =========================================================================
                    // 👟 SẢN PHẨM 2: Adidas2
                    // =========================================================================
                    new Product
                    {
                        Name = "Adidas Barricade 13",
                        Description = "JR7814",
                        Category = "Adidas",
                        Price = 99.00m,
                        
                        Variants = new List<ProductVariant>
                        {
                            new ProductVariant { Size = "40", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "41", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "42", Color = "", Quantity = DefaultQuantity }
                        },
                        
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "/images/barri_JR7814.jpeg", IsMainImage = true, DisplayOrder = 1 },
                            new ProductImage { ImageUrl = "/images/barri_JR7814(1).jpeg", IsMainImage = false, DisplayOrder = 2 },
                            new ProductImage { ImageUrl = "/images/barri_JR7814(2).jpeg", IsMainImage = false, DisplayOrder = 3 }
                        }
                    },

                    // =========================================================================
                    // 👟 SẢN PHẨM 3: Nike1
                    // =========================================================================
                    new Product
                    {
                        Name = "Nike Jordan",
                        Description = "553558_166",
                        Category = "Nike",
                        Price = 82.25m,
                        
                        // Lưu ý: Nếu muốn, bạn có thể thay đổi số lượng ở đây
                        Variants = new List<ProductVariant>
                        {
                            new ProductVariant { Size = "40", Color = "", Quantity = 5 }, // Ví dụ: Có 5 cái size 40
                            new ProductVariant { Size = "41", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "42", Color = "", Quantity = DefaultQuantity }
                        },
                        
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "/images/Jordan_553558_166.jpeg", IsMainImage = true, DisplayOrder = 1 },
                            new ProductImage { ImageUrl = "/images/Jordan_553558_166(1).jpeg", IsMainImage = false, DisplayOrder = 2 },
                            new ProductImage { ImageUrl = "/images/Jordan_553558_166(2).jpeg", IsMainImage = false, DisplayOrder = 3 }
                        }
                    },
                    
                    // VÀ TIẾP TỤC KHỞI TẠO TƯỜNG MINH CHO CÁC SẢN PHẨM KHÁC (Asics1, Asics2, NewBalance1, NewBalance2...)
                    
                    // Ví dụ: Asics1
                    new Product
                    {
                        Name = "Asics Court MZ",
                        Description = "1203A127_750",
                        Category = "Asics",
                        Price = 65.00m,
                        Variants = new List<ProductVariant>
                        {
                            new ProductVariant { Size = "40", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "41", Color = "", Quantity = DefaultQuantity },
                            new ProductVariant { Size = "42", Color = "", Quantity = DefaultQuantity }
                        },
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "/images/asics_1203A127_750.jpeg", IsMainImage = true, DisplayOrder = 1 },
                            new ProductImage { ImageUrl = "/images/asics_1203A127_750(1).jpeg", IsMainImage = false, DisplayOrder = 2 },
                            new ProductImage { ImageUrl = "/images/asics_1203A127_750(2).jpeg", IsMainImage = false, DisplayOrder = 3 }
                        }
                    }
                );
                
                context.SaveChanges();
            }
        }
    }
}