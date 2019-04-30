using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Database.Entities
{
    public class OrderServiceEty
    {
        public int Id { get; set; }        
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ClosedAt { get; set; }

        public OrderStatusEty Status { get; set; }
        public decimal Total { get; set; }

        public int CompanyId { get; set; }
        public virtual CompanyEty Company { get; set; }

        public int ClientId { get; set; }
        public virtual ClientEty Client { get; set; }

        public virtual List<OrderDetailEty> Items { get; set; }
        
    }
}
