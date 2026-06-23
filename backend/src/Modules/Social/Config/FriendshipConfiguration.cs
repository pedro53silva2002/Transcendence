using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Social.Model;

namespace Trippie.Modules.Social.Config;

internal sealed class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
	public void Configure(EntityTypeBuilder<Friendship> f)
	{
		f.ToTable("friendships", "social");
		f.HasKey(f => f.Id);

		f.Property(f => f.Id).HasColumnName("id").ValueGeneratedOnAdd();
		f.Property(f => f.UserId).HasColumnName("user_id").IsRequired();
		f.Property(f => f.FriendId).HasColumnName("friend_id").IsRequired();
		f.Property(f => f.CreatedAt).HasColumnName("created_at").IsRequired();
		
		f.HasIndex(f => new { f.UserId, f.FriendId }).IsUnique();
		f.ToTable(t => t.HasCheckConstraint("social_friendships_c_self_friendship", "user_id <> friend_id"));

		f.HasOne<UserId>()
			.WithMany()
			.HasForeignKey(f => f.UserId)
			.HasConstraintName("social_friendships_fk_user_1")
			.OnDelete(DeleteBehavior.Cascade);
		
		f.HasOne<FriendId>()
			.WithMany()
			.HasForeignKey(f => f.FriendId)
			.HasConstraintName("social_friendships_fk_user_2")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
