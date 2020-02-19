using Book.Domain.Entities;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.Infra.Data.Mappings
{
    public class BookCategoryMap : EntityMap<BookCategory>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<BookCategory> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}