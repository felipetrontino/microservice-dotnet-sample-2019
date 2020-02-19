using Book.Domain.CommandSide.Commands;
using Book.Domain.Common;
using Book.Domain.Extensions;
using Book.Infra.Data;
using Book.Models.Event;
using Framework.Core.Bus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Book.Domain.CommandSide.CommandHandlers
{
    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, bool>
    {
        private readonly DbBook _db;
        private readonly IBusPublisher _bus;

        public SaveBookCommandHandler(DbBook db, IBusPublisher bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task<bool> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            var inserted = false;
            var book = await _db.Books.FilterTitle(request.Title).FirstOrDefaultAsync(cancellationToken);

            if (book == null)
            {
                inserted = true;
                book = new Entities.Book();
            }

            book.Title = request.Title;
            book.Language = request.Language;
            book.AuthorName = request.Author;

            if (request.Category != null)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(x => x.Name == request.Category, cancellationToken);

                if (category == null)
                {
                    category = new Entities.BookCategory
                    {
                        Name = request.Category
                    };
                }

                if (!book.Categories.Any(x => x.Category.Name == category.Name))
                {
                    book.Categories.Add(new Entities.BookCategoryBook() { Category = category });
                }
            }

            if (inserted)
                await _db.AddAsync(book, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            var message = new UpdateBookEvent
            {
                Title = book.Title,
                Author = book.AuthorName,
                Language = book.Language
            };

            await _bus.PublishAsync(ContextNames.Exchange.Book, message);

            return true;
        }
    }
}