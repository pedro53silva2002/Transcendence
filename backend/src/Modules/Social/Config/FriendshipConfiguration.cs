using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Social.Model;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Social.Config;

internal sealed class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
	public void Configure(EntityTypeBuilder<Friendship> f)
	{
		f.ToTable("friendships", "social");
		f.HasKey(f => f.Id);

		f.Property(f => f.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd();

		f.Property(f => f.UserId)
			.HasColumnName("user_id")
			.IsRequired();

		f.Property(f => f.FriendId)
			.HasColumnName("friend_id")
			.IsRequired();

		f.Property(f => f.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("now()")
			.IsRequired();

		f.HasIndex(f => new { f.UserId, f.FriendId })
			.IsUnique()
			.HasDatabaseName("social_friendships_u_user_friend");

		f.HasIndex(f => f.FriendId)
			.HasDatabaseName("social_friendships_i_friend");

		f.ToTable(t => t.HasCheckConstraint(
			"social_friendships_c_self_friendship",
			"user_id <> friend_id"));

		f.HasOne<User>()
			.WithMany()
			.HasForeignKey(f => f.UserId)
			.HasConstraintName("social_friendships_fk_user")
			.OnDelete(DeleteBehavior.Cascade);
		
		f.HasOne<User>()
			.WithMany()
			.HasForeignKey(f => f.FriendId)
			.HasConstraintName("social_friendships_fk_friend")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
