using Bookstore.Domain.Entities;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infra.Data.Mappings
{
    public class OrderItemMap : EntityMap<OrderItem>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<OrderItem> builder)
        {
        }
    }
}