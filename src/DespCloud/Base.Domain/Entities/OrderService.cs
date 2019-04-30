using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities
{
    public class OrderService
    {
        public int Id { get; set; }        
        public int ClientId { get; set; }
        public Company Company { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ClosedAt { get; set; }
        public Client Client { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }

        public IEnumerable<OrderDetail> Items { get; set; } 
    }
}
