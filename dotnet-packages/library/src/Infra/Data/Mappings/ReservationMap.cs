using Framework.Data.EF;
using Library.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.Mappings
{
    public class ReservationMap : EntityMap<Reservation>
    {
        protected override void OnEntityBuild(EntityTypeBuilder<Reservation> builder)
        {
        }
    }
}