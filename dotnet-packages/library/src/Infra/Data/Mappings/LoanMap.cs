using Framework.Data.EF;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.Mappings
{
    public class LoanMap : EntityMap<Loan>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Loan> builder)
        {
        }
    }
}