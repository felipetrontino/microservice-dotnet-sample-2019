using Framework.Core.Entities;
using Library.Domain.Enums;

namespace Library.Domain.Entities
{
    public class Book : Entity
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public Language Language { get; set; }
    }
}