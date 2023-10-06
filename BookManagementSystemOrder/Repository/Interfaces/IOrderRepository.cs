using BookManagementSystem.Common;
using BookManagementSystem.Common.Models;

namespace BookManagementSystemOrder.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<ResultModel<string>> CreateOrder(OrderViewModel model);
        Task<ResultModel<Order>> GetOrders();
        Task<ResultModel<string>> GetBookListFromBookOrder();
        Task<ResultModel<BookOrder>> GetBookOrders();
        Task<ResultModel<string>> DeleteOrderById(int id);
    }
}