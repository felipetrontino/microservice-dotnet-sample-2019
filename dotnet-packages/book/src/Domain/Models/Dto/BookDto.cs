using Book.Domain.Enums;
using System;

namespace Book.Models.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public Language Language { get; set; }
    }
}