using System.Text.Json.Serialization;
using SportsStore.Infrastructure;
using Microsoft.AspNetCore.Http; // Cần thêm using này

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        // 🌟 SỬA LỖI 1: Cập nhật chữ ký AddItem để nhận ProductVariantID
        public override void AddItem(Product product, int quantity, int? productVariantId)
        {
            // Gọi phương thức AddItem của lớp cha (đã được sửa)
            base.AddItem(product, quantity, productVariantId);
            Session?.SetJson("Cart", this);
        }

        // 🌟 SỬA LỖI 2: Cập nhật chữ ký RemoveLine để nhận ProductVariantID
        public override void RemoveLine(Product product, int? productVariantId = null)
        {
            // Gọi phương thức RemoveLine của lớp cha (đã được sửa)
            base.RemoveLine(product, productVariantId);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }
    }
}