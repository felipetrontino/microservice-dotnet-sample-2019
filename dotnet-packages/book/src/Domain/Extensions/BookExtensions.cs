using System.Linq;

namespace Book.Domain.Extensions
{
    public static class BookExtensions
    {
        public static IQueryable<Entities.Book> FilterTitle(this IQueryable<Entities.Book> query, string title)
        {
            return query.Where(x => x.Title.Contains(title));
        }
    }
}