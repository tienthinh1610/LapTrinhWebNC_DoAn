namespace SportsStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        // Thêm virtual keyword cho các phương thức
        public virtual void AddItem(Product product, int quantity, int? productVariantId)
    {
        CartLine? line = Lines
            .Where(l => l.Product.ProductID == product.ProductID &&
                        l.ProductVariantID == productVariantId) // 🌟 SO SÁNH CẢ VARIANT ID
            .FirstOrDefault();

        if (line == null)
        {
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity,
                ProductVariantID = productVariantId // 🌟 GÁN VARIANT ID VÀO DÒNG MỚI
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

        public virtual void RemoveLine(Product product, int? productVariantId = null) =>
        Lines.RemoveAll(l => l.Product.ProductID == product.ProductID &&
                             (productVariantId == null || l.ProductVariantID == productVariantId));
    
    public decimal ComputeTotalValue() =>
        Lines.Sum(e => e.Product.Price * e.Quantity);

    public virtual void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
        // Trường này sẽ giữ ID của biến thể được chọn từ trang Details.
    public int? ProductVariantID { get; set; }
    }
}