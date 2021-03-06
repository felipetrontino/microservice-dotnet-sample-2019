﻿using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Domain.CommandSide.CommandHandlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
    {
        private readonly DbBookstore _db;

        public UpdateBookCommandHandler(DbBookstore db)
        {
            _db = db;
        }

        public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _db.Books.FirstOrDefaultAsync(x => x.Title == request.Title, cancellationToken);

            if (book != null)
            {
                book.Author = request.Author;
                book.Language = request.Language;

                await _db.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}