
using Core.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AccomodationConfig : IEntityTypeConfiguration<Accomodation>
    {
        public void Configure(EntityTypeBuilder<Accomodation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Place)
                .WithMany(x => x.Accomodations)
                .HasForeignKey(x => x.PlaceId);
        }
    }
}
