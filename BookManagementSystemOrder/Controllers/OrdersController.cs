using BookManagementSystem.Common;
using BookManagementSystem.Common.Models;
using BookManagementSystemOrder.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BookManagementSystemOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        public OrdersController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ResultModel<OrderResponse> result = new ResultModel<OrderResponse>();
            try
            {
                var orders = await _orderService.GetOrders();
                var listOfIds = new List<int>();
                foreach (var order in orders.data)
                {
                    listOfIds.Add(order.OrderId);
                }

                var bookIdList = await _orderService.GetBookListFromBookOrder();
                using (HttpClient httpClient = new HttpClient())
                {
                    if (!bookIdList.data.Any())
                    {
                        result.message = "No data";

                        return StatusCode((int)HttpStatusCode.BadRequest, result);
                    } 
                    string apiUrl = _configuration["MS2BaseUrl"]+ "api/books/GetBooksByIds";

                    string jsonIds = Newtonsoft.Json.JsonConvert.SerializeObject(bookIdList.data);
                     
                    var content = new StringContent(jsonIds, Encoding.UTF8, "application/json");
                     
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                     
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and process the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        ResultModel<Book> list = JsonConvert.DeserializeObject<ResultModel<Book>>(responseBody);
                        var bookOrders = await _orderService.GetBookOrders();
                        List<OrderResponse> respList = new List<OrderResponse>();

                        foreach (var item in bookOrders.data.Select(x=>x.OrderId).Distinct().ToList())
                        {
                            OrderResponse orderResponse = new OrderResponse();
                            orderResponse.OrderId = item;
                            orderResponse.OrderName = orders.data.Where(x => x.OrderId == item).Select(x => x.Name).FirstOrDefault();
                            respList.Add(orderResponse);
                        }
                        foreach(var item in respList)
                        {
                            var listOfBookOrders = bookOrders.data.Where(x => x.OrderId == item.OrderId).Select(x => x.BooktId).ToList();
                            var bookList = list.data.Where(x => listOfBookOrders.Contains(x.Id)).ToList();
                            item.Books = bookList;
                        }
                        result.data = respList;
                        result.success = true;
                        result.message = "Operation Successful";
                        result.status = "00";

                        return StatusCode((int)HttpStatusCode.OK, result);
                    }

                    return StatusCode((int)response.StatusCode, result);
                }
            }

            catch (Exception ex)
            {
                result.message = "Internal Server Error: " + ex.Message;

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }

        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderViewModel model)
        {
            ResultModel<string> response = new ResultModel<string>();
            try
            {
                var result = await _orderService.CreateOrder(model);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response.message = "Internal Server Error: " + ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrdersById(id);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
