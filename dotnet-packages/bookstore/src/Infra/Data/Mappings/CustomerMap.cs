using Bookstore.Domain.Entities;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infra.Data.Mappings
{
    public class CustomerMap : EntityMap<Customer>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Customer> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}