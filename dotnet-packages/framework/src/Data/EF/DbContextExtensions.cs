using Framework.Core.Config;
using Framework.Core.Entities;
using Framework.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace Framework.Data.EF
{
    public static class DbContextExtensions
    {
        public static void Delete<TEntity>(this DbContext dbContext, TEntity entity)
             where TEntity : class, IVirtualDeletedEntity
        {
            entity.IsDeleted = true;
            dbContext.Update(entity);
        }

        public static void Delete<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
              where TEntity : class, IVirtualDeletedEntity
        {
            entity.IsDeleted = true;
            dbSet.Update(entity);
        }

        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }

        public static IServiceCollection AddDb<TContext>(this IServiceCollection serviceCollection, string connectionString)
            where TContext : DbContext
        {
            var sqlBuilder = new SqlConnectionStringBuilder(connectionString);

            if (sqlBuilder.InitialCatalog == string.Empty)
                sqlBuilder.InitialCatalog = $"{Configuration.Stage.Get().GetName().ToUpper()}_{Configuration.AppName.Get().ToUpper()}";

            var conn = sqlBuilder.ToString();

            return serviceCollection.AddDbContext<TContext>(opt => opt.UseSqlServer(conn, x => x.UseNetTopologySuite()));
        }
    }
}