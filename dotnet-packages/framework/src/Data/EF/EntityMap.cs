using Framework.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EF
{
    public abstract class EntityMap<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            var type = typeof(T);

            builder.Property(nameof(Entity.Id)).ValueGeneratedNever();

            if (typeof(IAuditEntity).IsAssignableFrom(type))
            {
                builder.HasIndex(nameof(Entity.InsertedAt));
                builder.HasIndex(nameof(Entity.UpdatedAt));
                builder.HasIndex(nameof(Entity.DeletedAt));
            }

            if (typeof(IVirtualDeletedEntity).IsAssignableFrom(type))
            {
                builder.HasIndex(nameof(Entity.IsDeleted));
            }

            if (typeof(IConcurrencyEntity).IsAssignableFrom(type))
            {
                builder.Property(nameof(Entity.RowVersion)).IsRowVersion();
            }

            OnEntityBuild(builder);
        }

        protected virtual void OnEntityBuild(EntityTypeBuilder<T> builder)
        {
        }
    }
}