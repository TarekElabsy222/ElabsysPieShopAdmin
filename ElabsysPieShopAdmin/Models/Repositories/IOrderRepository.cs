namespace ElabsysPieShopAdmin.Models.Repositories
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllOredersWithDetailsAsync();
        public Task<Order?> GetOrderWithDetails(int? orderId);
    }
}
