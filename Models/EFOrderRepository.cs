using Microsoft.EntityFrameworkCore;
using System.Linq; // Đảm bảo có using này

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext context;

        public EFOrderRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        // 🌟 SỬA 1: Bỏ ThenInclude(l => l.Product)
        // OrderLine không còn tham chiếu Product, nên không cần tải Product
        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines); // Chỉ cần tải các dòng đơn hàng (OrderLine)

        public void SaveOrder(Order order)
{
    // Kiểm tra và xử lý OrderLineID CHỈ KHI LÀ ĐƠN HÀNG MỚI
    if (order.OrderID == 0)
    {
        // Khi thêm đơn hàng mới, đảm bảo OrderLineID là 0
        // để EF Core hiểu rằng đây là các dòng mới cần được chèn (INSERT)
        foreach (OrderLine line in order.Lines)
        {
            line.OrderLineID = 0;
        }
        context.Orders.Add(order); // Đơn hàng mới
    }
    else
    {
        // Khi CẬP NHẬT đơn hàng đã tồn tại (ví dụ: cập nhật trạng thái Shipped),
        // KHÔNG được chạm vào OrderLineID.
        // Chỉ cần gọi SaveChanges() để cập nhật các thuộc tính đã thay đổi (như Shipped)
        // EF Core sẽ tự động phát hiện và UPDATE các thuộc tính đã được thay đổi.
        context.Orders.Update(order); // Hoặc bạn có thể không cần dòng này nếu đối tượng đã được theo dõi
    }
    
    // Lưu thay đổi
    context.SaveChanges();
    
}

    }
}