using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Travel.Model;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Travel.Config;

internal sealed class TripMembersConfiguration : IEntityTypeConfiguration<TripMembers>
{
	public void Configure(EntityTypeBuilder<TripMembers> tm)
	{
		tm.ToTable("trip_members", "travel");
		tm.HasKey(tm => tm.Id);

		tm.Property(tm => tm.Id).HasColumnName("id").ValueGeneratedOnAdd();
		tm.Property(tm => tm.TripId).HasColumnName("trip_id").IsRequired();
		tm.Property(tm => tm.UserId).HasColumnName("user_id").IsRequired();
		tm.Property(tm => tm.Role).HasColumnName("role").IsRequired();
		tm.Property(tm => tm.JoinedAt).HasColumnName("joined_at");
		tm.Property(tm => tm.UpdatedAt).HasColumnName("updated_at");

		tm.HasIndex(tm => new { tm.TripId, tm.UserId }).IsUnique();

		//Navigation to Trip
		// tm.HasOne<Trip>()
		// 	.WithMany()
		// 	.HasForeignKey(t => t.TripId)
		// 	.HasConstraintName("travel_trip_members_fk_trip")
		// 	.OnDelete(DeleteBehavior.Cascade);

		// Navigation to User (use type-based navigation if TripMembers doesn't expose a User property)
		tm.HasOne<User>()
			.WithMany()
			.HasForeignKey(tm => tm.UserId)
			.HasConstraintName("travel_trip_members_fk_user")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
