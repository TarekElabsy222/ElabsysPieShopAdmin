using ElabsysPieShopAdmin.Models;
using ElabsysPieShopAdmin.Models.Repositories;
using ElabsysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElabsysPieShopAdmin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index(int? orderId, int? orderDetailId)
        {
            OrderIndexViewModel orderIndexViewModel = new()
            {
                Orders = await _orderRepository.GetAllOredersWithDetailsAsync()
            };

            if (orderId != null)
            {
                Order selectedOrder = orderIndexViewModel.Orders.Where(o => o.OrderId == orderId).Single();
                orderIndexViewModel.OrderDetails = selectedOrder.OrderDetails;
                orderIndexViewModel.SelectedOrderId = orderId;
            }

            if (orderDetailId != null)
            {
                var selectedOrderDetail = orderIndexViewModel.OrderDetails.Where(od => od.OrderDetailId == orderDetailId).Single();
                orderIndexViewModel.Pies = new List<Pie>() { selectedOrderDetail.Pie };
                orderIndexViewModel.SelectedOrderDetailId = orderDetailId;
            }
            return View(orderIndexViewModel);

        }
        public async Task<IActionResult> Details(int? orderId)
        {
            var result = await _orderRepository.GetOrderWithDetails(orderId);

            return View(result);
        }
    }
}
