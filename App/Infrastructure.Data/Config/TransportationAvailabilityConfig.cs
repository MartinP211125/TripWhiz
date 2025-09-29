
using Core.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class TransportationAvailabilityConfig : IEntityTypeConfiguration<TransportationAvailability>
    {
        public void Configure(EntityTypeBuilder<TransportationAvailability> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Seat)
                .WithMany(x => x.TransportationAvailabilities)
                .HasForeignKey(x => new { x.TransportationId, x.SeatId });
        }
    }
}
