using Book.Domain.Enums;
using Book.Domain.QuerySide.Queries;
using Book.Infra.Data;
using Book.Models.Dto;
using Framework.Core.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Book.Domain.QuerySide.QueryHandlers
{
    public class BookQueryHandler :
        IRequestHandler<GetBookQuery, BookDto>,
        IRequestHandler<ListBookFilteringQuery, PagedResponse<BookDto>>
    {
        private readonly DbBook _db;

        public BookQueryHandler(DbBook db)
        {
            _db = db;
        }

        public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            return await _db.Books
                .Where(x => x.Id == request.Id)
                .Select(x => new BookDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.AuthorName,
                    Language = x.Language
                }).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedResponse<BookDto>> Handle(ListBookFilteringQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Books.AsQueryable();

            query = FilterQuery(query, request);

            return await query.ToPagedResponseAsync(request, x => new BookDto
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.AuthorName,
                Language = x.Language
            }, cancellationToken);
        }

        private static IQueryable<Entities.Book> FilterQuery(IQueryable<Entities.Book> query, ListBookFilteringQuery filter)
        {
            if (filter.Title != null)
                query = query.Where(_ => _.Title == filter.Title);

            if (filter.Category != null)
                query = query.Where(_ => _.Categories.Any(c => c.Category.Name == filter.Category));

            if (filter.Language != null && filter.Language != Language.Unknown)
                query = query.Where(_ => _.Language == filter.Language);

            return query;
        }
    }
}