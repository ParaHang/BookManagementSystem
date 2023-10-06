using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementSystem.Common.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderName { get;set; }
        public List<int> BookIds { get; set; }
    }
}
