using Framework.Core.Config;
using Microsoft.EntityFrameworkCore;

namespace Framework.Data.EF
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public string Schema { get; protected set; } = Configuration.AppName.Get();

        public virtual void EnsureSeedData()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.RemovePluralizingTableNameConvention();
        }
    }
}