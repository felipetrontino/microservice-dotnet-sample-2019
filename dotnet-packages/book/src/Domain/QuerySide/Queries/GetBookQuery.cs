using Book.Models.Dto;
using MediatR;
using System;

namespace Book.Domain.QuerySide.Queries
{
    public class GetBookQuery : IRequest<BookDto>
    {
        public Guid Id { get; set; }
    }
}