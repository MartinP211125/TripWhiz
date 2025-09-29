
using Core.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class RoomConfig : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(x => new { x.AccomodationId, x.Id });
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Accomodation)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.AccomodationId);
        }
    }
}
