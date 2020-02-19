using Framework.Core.Entities;

namespace Bookstore.Domain.Entities
{
    public class OrderItem : Entity, IValueEntity
    {
        public Book Book { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public double Quantity { get; set; }

        public double Total { get; set; }
    }
}