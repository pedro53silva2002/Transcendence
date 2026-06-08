using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Travel.Model;

namespace Trippie.Modules.Travel.Config;

internal sealed class TripCountryConfiguration : IEntityTypeConfiguration<TripCountry>
{
    public void Configure(EntityTypeBuilder<TripCountry> b)
    {
        b.ToTable("trips_countries", "travel");
        b.HasKey(tc => new { tc.TripId, tc.CountryId });

        b.Property(tc => tc.TripId).HasColumnName("trip_id");
        b.Property(tc => tc.CountryId).HasColumnName("country_id");

        b.HasOne(tc => tc.Trip)
            .WithMany()
            .HasForeignKey(tc => tc.TripId)
            .HasConstraintName("travel_trip_countries_fk_trip")
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(tc => tc.Country)
            .WithMany()
            .HasForeignKey(tc => tc.CountryId)
            .HasConstraintName("travel_trip_countries_fk_country")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
