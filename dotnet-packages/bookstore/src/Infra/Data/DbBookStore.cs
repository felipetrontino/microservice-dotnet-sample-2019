using Bookstore.Domain.Entities;
using Bookstore.Infra.Data.Mappings;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data
{
    public class DbBookstore : EfDbContext
    {
        public DbBookstore(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}