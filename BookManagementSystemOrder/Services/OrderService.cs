using BookManagementSystem.Common;
using BookManagementSystem.Common.Models;
using BookManagementSystemOrder.Interfaces;
using BookManagementSystemOrder.Repository.Interfaces;

namespace BookManagementSystemOrder.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<ResultModel<string>> CreateOrder(OrderViewModel model)
        {
            return await _orderRepository.CreateOrder(model);
        }
        public async Task<ResultModel<Order>> GetOrders()
        {
            return await _orderRepository.GetOrders();
        }
        public async Task<ResultModel<string>> GetBookListFromBookOrder()
        {
            return await _orderRepository.GetBookListFromBookOrder();
        }
        public async Task<ResultModel<BookOrder>> GetBookOrders()
        {
            return await _orderRepository.GetBookOrders();
        }
        public async Task<ResultModel<string>> DeleteOrdersById(int id)
        {
            return await _orderRepository.DeleteOrderById(id);
        }
    }
}
