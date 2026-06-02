using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Config;

internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
	public void Configure(EntityTypeBuilder<City> b)
	{
		b.ToTable("cities", "auth");
		b.HasKey(c => c.Id);

		b.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
		b.Property(c => c.Name).HasColumnName("name").IsRequired();
		b.Property(c => c.CountryId).HasColumnName("country_id").IsRequired();

		// to match UNIQUE(name,country_id) for the database
		b.HasIndex(c => new { c.Name, c.CountryId }).IsUnique();

		b.HasOne(c => c.Country)
			.WithMany(c => c.Cities)
			.HasForeignKey(c => c.CountryId)
			.HasConstraintName("auth_cities_fk_country")
			.OnDelete(DeleteBehavior.Cascade);
    }
}