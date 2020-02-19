using Book.Domain.Entities;
using Book.Infra.Data.Mappings;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Book.Infra.Data
{
    public class DbBook : EfDbContext
    {
        public DbBook(DbContextOptions options)
             : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookMap());
            modelBuilder.ApplyConfiguration(new BookCategoryMap());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Domain.Entities.Book> Books { get; set; }

        public DbSet<BookCategory> Categories { get; set; }
    }
}