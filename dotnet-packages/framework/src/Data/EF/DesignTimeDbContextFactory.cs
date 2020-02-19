using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Framework.Data.EF
{
    public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public abstract string Schema { get; }

        public virtual string ConnectionString { get; } = "Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True;";

        public TContext CreateDbContext(string[] args)
        {
            var builder = GetOptionsBuilder()
                            .UseSqlServer(ConnectionString, x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema)
                                                                  .UseNetTopologySuite());

            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }

        public virtual DbContextOptionsBuilder GetOptionsBuilder()
        {
            return new DbContextOptionsBuilder<TContext>();
        }
    }
}