using Microsoft.EntityFrameworkCore;
using Trippie.Modules.Auth.Model;
using Trippie.Modules.Travel.Model;

namespace Trippie.Common.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    //Everytime we create a module/submodule, add here the set.
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
	public DbSet<TripCountry> TripCountries => Set<TripCountry>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
