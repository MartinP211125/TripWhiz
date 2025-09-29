using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Entities;


namespace Infrastructure.Data.Config
{
    public class TripWhizUserConfig : IEntityTypeConfiguration<TripWhizUser>
    {
        public void Configure(EntityTypeBuilder<TripWhizUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
