using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementSystem.Common.Models
{
    public class BookOrder
    {
        public string Id { get; set; }
        public int BooktId { get; set; }
        public int OrderId { get; set; }
    }
}
