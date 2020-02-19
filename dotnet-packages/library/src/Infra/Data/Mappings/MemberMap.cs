using Framework.Data.EF;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.Mappings
{
    public class MemberMap : EntityMap<Member>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Member> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}