using Framework.Core.Entities;

namespace Library.Domain.Entities
{
    public class Copy : Entity
    {
        public string Number { get; set; }

        public Book Book { get; set; }
    }
}