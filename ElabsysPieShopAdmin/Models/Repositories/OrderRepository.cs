
using ElabsysPieShopAdmin.Data;
using Microsoft.EntityFrameworkCore;

namespace ElabsysPieShopAdmin.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOredersWithDetailsAsync()
            => await _context.Orders.Include(o=> o.OrderDetails).ThenInclude(p=>p.Pie).OrderBy(o=>o.OrderId).ToListAsync();

        public async Task<Order?> GetOrderWithDetails(int? orderId)
         => await _context.Orders.Include(o=>o.OrderDetails).ThenInclude(p=>p.Pie).OrderBy(o=>o.OrderId).Where(p=>p.OrderId == orderId).FirstOrDefaultAsync();

    }
}
