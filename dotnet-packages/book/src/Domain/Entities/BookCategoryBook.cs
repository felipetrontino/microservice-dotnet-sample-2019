using Framework.Core.Entities;

namespace Book.Domain.Entities
{
    public class BookCategoryBook : Entity
    {
        public Book Book { get; set; }

        public BookCategory Category { get; set; }
    }
}