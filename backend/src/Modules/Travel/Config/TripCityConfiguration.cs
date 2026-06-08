using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Travel.Model;

namespace Trippie.Modules.Travel.Config;

internal sealed class TripCityConfiguration : IEntityTypeConfiguration<TripCity>
{
    public void Configure(EntityTypeBuilder<TripCity> b)
    {
        b.ToTable("trips_cities", "travel");
        b.HasKey(tc => new { tc.TripId, tc.CityId });

        b.Property(tc => tc.TripId).HasColumnName("trip_id");
        b.Property(tc => tc.CityId).HasColumnName("city_id");

        // b.HasOne(tc => tc.Trip)
        //     .WithMany()
        //     .HasForeignKey(tc => tc.TripId)
        //     .HasConstraintName("travel_trips_cities_fk_trip")
        //     .OnDelete(DeleteBehavior.Cascade);

        // b.HasOne(tc => tc.City)
        //     .WithMany()
        //     .HasForeignKey(tc => tc.CityId)
        //     .HasConstraintName("travel_trips_cities_fk_city")
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}
