using BookManagementSystem.Common;
using BookManagementSystem.Common.Models;

namespace BookManagementSystemOrder.Interfaces
{
    public interface IOrderService
    {
        Task<ResultModel<string>> CreateOrder(OrderViewModel model);
        Task<ResultModel<Order>> GetOrders();
        Task<ResultModel<string>> GetBookListFromBookOrder();
        Task<ResultModel<BookOrder>> GetBookOrders();
    }
}
