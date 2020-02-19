using Framework.Core.Entities;
using System.Collections.Generic;

namespace Book.Domain.Entities
{
    public class BookCategory : Entity
    {
        public string Name { get; set; }

        public List<BookCategoryBook> Books { get; set; } = new List<BookCategoryBook>();
    }
}