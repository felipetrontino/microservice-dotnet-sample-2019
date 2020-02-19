using Framework.Data.EF;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.Mappings
{
    public class CopyMap : EntityMap<Copy>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Copy> builder)
        {
            builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}