using Framework.Core.Config;
using Framework.Core.Entities;
using Framework.Core.Logging.LoggerFactory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Data.EF
{
    public abstract class EfDbContext : BaseDbContext
    {
        protected EfDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            return await TrySaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void OnBeforeSaveChanges()
        {
            this.ChangeTracker.Configure(DateTime.UtcNow);
        }

        protected virtual void OnAfterSaveChanges()
        {
        }

        private async Task<int> TrySaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();

            var ret = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            OnAfterSaveChanges();

            return await ret;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var type = entityType.ClrType;

                var isVirtualDeleted = typeof(IVirtualDeletedEntity).IsAssignableFrom(type);

                if (isVirtualDeleted)
                    SetVirtualDeletedFilterMethod.MakeGenericMethod(type).Invoke(this, new object[] { modelBuilder });
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = typeof(IValueEntity).IsAssignableFrom(relationship.DeclaringEntityType.ClrType)
                    ? DeleteBehavior.Cascade
                    : DeleteBehavior.Restrict;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ReplaceService<IEntityMaterializerSource, CustomEntityMaterializerSource>();

            if (!Configuration.DebuggingSql.Get()) return;

            optionsBuilder.EnableSensitiveDataLogging();
            var log = new LoggerFactory(new[] { new CustomLoggerProvider() });
            optionsBuilder.UseLoggerFactory(log);
        }

        private class CustomEntityMaterializerSource : EntityMaterializerSource
        {
            private static readonly MethodInfo NormalizeMethod =
                typeof(DateTimeMapper).GetTypeInfo().GetMethod(nameof(DateTimeMapper.Normalize));

            private static readonly MethodInfo NormalizeNullableMethod =
                typeof(DateTimeMapper).GetTypeInfo().GetMethod(nameof(DateTimeMapper.NormalizeNullable));

            public CustomEntityMaterializerSource([NotNullAttribute] EntityMaterializerSourceDependencies dependencies) : base(dependencies)
            {
            }

            public override Expression CreateReadValueExpression
                (Expression valueBufferExpression, Type type, int index, IPropertyBase property)
            {
                if (type == typeof(DateTime))
                {
                    return Expression.Call(NormalizeMethod, base.CreateReadValueExpression(valueBufferExpression, type, index, property));
                }

                if (type == typeof(DateTime?))
                {
                    return Expression.Call(NormalizeNullableMethod, base.CreateReadValueExpression(valueBufferExpression, type, index, property));
                }

                return base.CreateReadValueExpression(valueBufferExpression, type, index, property);
            }

            private static class DateTimeMapper
            {
                public static DateTime Normalize(DateTime value)
                {
                    return DateTime.SpecifyKind(value, DateTimeKind.Utc);
                }

                public static DateTime? NormalizeNullable(DateTime? value)
                {
                    return !value.HasValue ? null : (DateTime?)DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
                }
            }
        }

        private static readonly MethodInfo SetVirtualDeletedFilterMethod = typeof(EfDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance).Single(t => t.IsGenericMethod && t.Name == "SetVirtualDeletedFilter");

        public void SetVirtualDeletedFilter<T>(ModelBuilder builder)
            where T : class, IVirtualDeletedEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}