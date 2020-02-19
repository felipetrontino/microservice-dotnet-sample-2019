using Framework.Data.EF;
using Library.Domain.Entities;
using Library.Entities;
using Library.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class DbLibrary : EfDbContext
    {
        public DbLibrary(DbContextOptions options)
             : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookMap());
            modelBuilder.ApplyConfiguration(new CopyMap());
            modelBuilder.ApplyConfiguration(new LoanMap());
            modelBuilder.ApplyConfiguration(new MemberMap());
            modelBuilder.ApplyConfiguration(new ReservationMap());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Copy> Copies { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}