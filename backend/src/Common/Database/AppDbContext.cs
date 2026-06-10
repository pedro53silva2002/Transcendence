using Microsoft.EntityFrameworkCore;
using Trippie.Modules.Auth.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Model;

namespace Trippie.Common.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    //Everytime we create a module/submodule, add here the set.
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
	public DbSet<TripMembers> TripMembers => Set<TripMembers>();
	public DbSet<Trip> Trips => Set<Trip>();
	public DbSet<TripCountry> TripCountries => Set<TripCountry>();
    public DbSet<TripCity> TripCities => Set<TripCity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		modelBuilder.HasPostgresEnum<TripVisibility>(name: "trip_visibility");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		modelBuilder.HasPostgresEnum<TripMemberRole>(name: "member_role");
		
		modelBuilder.Entity<TripCountry>(e =>
		{
			e.HasKey(tc => new { tc.TripId, tc.CountryId });

			e.HasOne(tc => tc.Trip)
			.WithOne(t => t.TripCountries);

			e.HasOne(tc => tc.Country)
			.WithMany()
			.HasForeignKey(tc => tc.CountryId);
		});

		modelBuilder.Entity<TripCity>(e =>
		{
			e.HasKey(tc => new { tc.TripId, tc.CityId });

			e.HasOne(tc => tc.Trip)
			.WithMany(t => t.TripCities)
			.HasForeignKey(tc => tc.TripId);

			e.HasOne(tc => tc.City)
			.WithMany()
			.HasForeignKey(tc => tc.CityId);
		});
	}
}
