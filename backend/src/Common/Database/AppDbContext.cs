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
	public DbSet<Trip> Trips => Set<Trip>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		modelBuilder.HasPostgresEnum<TripVisibility>(name: "trip_visibility");
	}
}
