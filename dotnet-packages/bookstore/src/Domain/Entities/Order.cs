using Bookstore.Domain.Enums;
using Framework.Core.Entities;
using System;
using System.Collections.Generic;

namespace Bookstore.Domain.Entities
{
    public class Order : Entity
    {
        public string Number { get; set; }
        public Customer Customer { get; set; }

        public StatusOrder Status { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public DateTime CreateDate { get; set; }
    }
}