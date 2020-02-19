using Bookstore.Domain.Enums;
using Framework.Core.Entities;
using System.Collections.Generic;

namespace Bookstore.Domain.Entities
{
    public class Book : Entity
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public Language Language { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}