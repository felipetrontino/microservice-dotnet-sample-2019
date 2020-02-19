using Book.Domain.Enums;
using Framework.Core.Entities;
using System.Collections.Generic;

namespace Book.Domain.Entities
{
    public class Book : Entity
    {
        public string Title { get; set; }

        public string AuthorName { get; set; }

        public Language Language { get; set; }

        public List<BookCategoryBook> Categories { get; set; } = new List<BookCategoryBook>();
    }
}