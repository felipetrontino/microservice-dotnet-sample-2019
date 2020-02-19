using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Tools.DbContext
{
    public class DbMigrations : DbBookstore
    {
        public DbMigrations(DbContextOptions options)
            : base(options)
        {
            Schema = nameof(Bookstore);
        }
    }
}