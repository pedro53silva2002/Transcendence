using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Travel.Model;

namespace  Trippie.Modules.Travel.Config;

internal sealed class ItineraryConfiguration : IEntityTypeConfiguration<Itinerary>
{
    public void Configure(EntityTypeBuilder<Itinerary> b)
    {
        b.ToTable("itineraries", "travel");
        b.HasKey(i => i.Id);

        b.Property(i => i.Id).HasColumnName("id").ValueGeneratedOnAdd();
        b.Property(i => i.TripId).HasColumnName("trip_id").IsRequired();
        b.Property(i => i.Title).HasColumnName("title").IsRequired();
        b.Property(i => i.Description).HasColumnName("description");
        b.Property(i => i.ExpectedPrice).HasColumnName("expected_price").IsRequired();
        b.Property(i => i.Day).HasColumnName("day").IsRequired();
        b.Property(i => i.CreatedBy).HasColumnName("created_by").IsRequired();
        b.Property(i => i.CreatedAt).HasColumnName("created_at").IsRequired().HasDefaultValueSql("NOW()");
        b.Property(i => i.UpdatedAt).HasColumnName("updated_at");

        /* To remove after Coletes finish this implementation
        b.HasOne(i => i.Trip)
            .WithMany(t => t.Itineraries)
            .HasForeignKey(i => i.TripId)
            .HasConstraintName("travel_itineraries_fk_trip")
            .OnDelete(DeleteBehavior.Cascade); */
    }
}
