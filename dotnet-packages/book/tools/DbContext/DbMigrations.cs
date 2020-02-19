using Book.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Book.Tools.DbContext
{
    public class DbMigrations : DbBook
    {
        public DbMigrations(DbContextOptions options)
            : base(options)
        {
            Schema = nameof(Book);
        }
    }
}