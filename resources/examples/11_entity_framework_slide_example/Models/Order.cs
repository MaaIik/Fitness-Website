using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
    public class Order
    {
        public Order()
        {
            OrderTime = DateTime.UtcNow;
            OrderLines = new List<OrderLine>();
        }

        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderTime { get; set; }
        
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}
