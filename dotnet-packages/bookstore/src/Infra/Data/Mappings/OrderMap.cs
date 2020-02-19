using Bookstore.Domain.Entities;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infra.Data.Mappings
{
    public class OrderMap : EntityMap<Order>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}