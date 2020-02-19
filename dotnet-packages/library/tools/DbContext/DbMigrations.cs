using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Tools.DbContext
{
    public class DbMigrations : DbLibrary
    {
        public DbMigrations(DbContextOptions options)
            : base(options)
        {
            Schema = nameof(Library);
        }
    }
}