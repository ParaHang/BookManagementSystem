using BookManagementSystem.Common;
using BookManagementSystem.Common.Models;
using BookManagementSystemOrder.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BookManagementSystemOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
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
                    // Specify the API URL you want to call
                    string apiUrl = "https://localhost:7777/api/books/GetBooksByIds";
                     
                    string jsonIds = Newtonsoft.Json.JsonConvert.SerializeObject(bookIdList.data);

                    // Create a StringContent object with the JSON data
                    var content = new StringContent(jsonIds, Encoding.UTF8, "application/json");

                    // Send a POST request to the API with the list of integers as parameters
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    // Check if the response is successful (HTTP status code 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and process the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        ResultModel<Book> list = JsonConvert.DeserializeObject<ResultModel<Book>>(responseBody);
                        var bookOrders = await _orderService.GetBookOrders();
                        List<OrderResponse> respList = new List<OrderResponse>();

                        foreach(var item in bookOrders.data)
                        {
                            OrderResponse orderResponse = new OrderResponse();
                            orderResponse.OrderId = item.OrderId;
                            orderResponse.OrderName = orders.data.Where(x => x.OrderId == item.OrderId).Select(x => x.Name).FirstOrDefault();
                            orderResponse.Books = list.data.Where(x=>x.Id == item.BooktId).ToList();
                            respList.Add(orderResponse);
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
    }
}
