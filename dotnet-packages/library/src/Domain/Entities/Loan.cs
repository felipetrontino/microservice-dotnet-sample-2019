using Framework.Core.Entities;
using System;

namespace Library.Domain.Entities
{
    public class Loan : Entity, IValueEntity
    {
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Copy Copy { get; set; }
    }
}