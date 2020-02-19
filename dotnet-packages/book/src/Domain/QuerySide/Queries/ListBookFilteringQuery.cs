using Book.Domain.Enums;
using Book.Models.Dto;
using Framework.Core.Pagination;
using MediatR;

namespace Book.Domain.QuerySide.Queries
{
    public class ListBookFilteringQuery : PagedRequest, IRequest<PagedResponse<BookDto>>
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public Language? Language { get; set; }
    }
}