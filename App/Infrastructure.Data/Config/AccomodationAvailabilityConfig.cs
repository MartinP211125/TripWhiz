
using Core.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AccomodationAvailabilityConfig : IEntityTypeConfiguration<AccomodationAvailability>
    {
        public void Configure(EntityTypeBuilder<AccomodationAvailability> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Room)
                .WithMany(x => x.AccomodationAvailabilities)
                .HasForeignKey(x => new { x.AccomodationId, x.RoomId });
        }
    }
}
