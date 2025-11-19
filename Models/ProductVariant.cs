namespace SportsStore.Models
{
    public class ProductVariant
    {
        public int ProductVariantID { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Quantity { get; set; }
        
        // ---- THUỘC TÍNH ẢNH BIẾN THỂ ----
        public string ImageUrl { get; set; } = string.Empty;
        
        // --- KHÓA NGOẠI ---
        public long? ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}