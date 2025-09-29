
using Core.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SeatConfig : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(x => new {x.TransportationId, x.Id});
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Transportation)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.TransportationId);
        }
    }
}
