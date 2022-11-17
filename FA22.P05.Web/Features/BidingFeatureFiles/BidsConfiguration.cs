using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FA22.P05.Web.Features.BidingFeatureFiles
{
    public class BidsConfiguration : IEntityTypeConfiguration<Bids>
    {
        public void Configure(EntityTypeBuilder<Bids> builder)
        {
            builder.HasOne(x => x.Listing)
                .WithMany()
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
