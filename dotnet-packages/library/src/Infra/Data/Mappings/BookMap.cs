using Framework.Data.EF;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.Mappings
{
    public class BookMap : EntityMap<Book>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(p => p.Title).IsUnique();
        }
    }
}