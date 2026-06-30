using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Auth.Model;
using Trippie.Modules.Travel.Model;

namespace Trippie.Modules.Auth.Config;

internal sealed class VisitedCountriesConfiguration : IEntityTypeConfiguration<VisitedCountry>
{
    public void Configure(EntityTypeBuilder<VisitedCountry> vc)
	{
		vc.ToTable("visited_countries", "auth");

		vc.Property(vc => vc.UserId)
			.HasColumnName("user_id");

		vc.Property(vc => vc.CountryId)
			.HasColumnName("country_id");

		vc.Property(x => x.SourceTripId)
            .HasColumnName("source_trip_id")
            .IsRequired(false);
		
		vc.Property(vc => vc.AddedAt)
			.HasColumnName("added_at")
			.HasDefaultValueSql("NOW()");

		vc.Property(vc => vc.DeletedAt)
			.HasColumnName("deleted_at")
			.IsRequired(false);

		vc.HasOne(vc => vc.User)
			.WithMany()
			.HasForeignKey(vc => vc.UserId)
			.HasConstraintName("auth_visited_countries_fk_user")
			.OnDelete(DeleteBehavior.Cascade);
		
		vc.HasOne(vc => vc.Country)
			.WithMany()
			.HasForeignKey(vc => vc.CountryId)
			.HasConstraintName("auth_visited_countries_fk_country")
			.OnDelete(DeleteBehavior.Restrict);

		vc.HasOne<Trip>()
            .WithMany()
            .HasForeignKey(x => x.SourceTripId)
            .HasConstraintName("auth_visited_countries_fk_source_trip")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

		vc.HasIndex(x => x.SourceTripId)
            .HasDatabaseName("ix_auth_visited_countries_source_trip_id");
	}
}
