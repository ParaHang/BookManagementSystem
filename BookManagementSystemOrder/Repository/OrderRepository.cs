using BookManagementSystem.Common;
using BookManagementSystem.Common.Models;
using BookManagementSystemOrder.DBContext;
using BookManagementSystemOrder.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystemOrder.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<ResultModel<string>> CreateOrder(OrderViewModel model)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                Order order = new Order()
                {
                    OrderId = model.OrderId,
                    Name = model.OrderName
                };
                await _context.Orders.AddAsync(order);
                foreach(var item in model.BookIds)
                {
                    BookOrder bookOrder = new BookOrder()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BooktId = item,
                        OrderId = model.OrderId
                    };
                    await _context.BookOrders.AddAsync(bookOrder);
                }
                await _context.SaveChangesAsync();

                result.message = "Operation Successful";
                result.success = true;
                result.status = "00";

            }
            catch(Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;
            } 
            return result;
        }
        public async Task<ResultModel<Order>> GetOrders()
        {
            ResultModel<Order> result = new ResultModel<Order>();
            try
            {
                var orders = await _context.Orders.ToListAsync();
                if(orders != null && orders.Any())
                {
                    result.status = "00";
                    result.success = true;
                    result.data = orders;
                    result.message = "Operation Successful";
                }
                else
                {
                    result.message = "No data";
                }
            }
            catch(Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }
        public async Task<ResultModel<BookOrder>> GetBookOrders()
        {
            ResultModel<BookOrder> result = new ResultModel<BookOrder>();
            try
            {
                var orders = await _context.BookOrders.GroupBy(x=>x.OrderId).Select(group => group.First()).ToListAsync();
                if (orders != null && orders.Any())
                {
                    result.status = "00";
                    result.success = true;
                    result.data = orders;
                    result.message = "Operation Successful";
                }
                else
                {
                    result.message = "No data";
                }
            }
            catch (Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }
        public async Task<ResultModel<string>> GetBookListFromBookOrder()
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                var listOfBookId = await _context.BookOrders.Select(x=>x.BooktId.ToString()).Distinct().ToListAsync();
                if(listOfBookId!= null && listOfBookId.Any())
                {
                    result.data = listOfBookId;
                    result.success =true;
                    result.message = "Operation Successful";
                    result.status = "00";
                }
                else
                {
                    result.message = "No data";
                }
            }
            catch(Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;
            }
            return result;
        }
    }
}
