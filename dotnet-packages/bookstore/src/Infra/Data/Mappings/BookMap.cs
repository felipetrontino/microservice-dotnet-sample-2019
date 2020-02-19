using Bookstore.Domain.Entities;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infra.Data.Mappings
{
    public class BookMap : EntityMap<Book>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(p => p.Title).IsUnique();
        }
    }
}