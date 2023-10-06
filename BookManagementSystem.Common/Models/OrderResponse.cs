using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementSystem.Common.Models
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
