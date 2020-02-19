using Framework.Data.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.Infra.Data.Mappings
{
    public class BookMap : EntityMap<Domain.Entities.Book>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Domain.Entities.Book> builder)
        {
            builder.HasIndex(p => p.Title).IsUnique();
        }
    }
}