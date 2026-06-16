using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using social.Model;

namespace Trippie.Modules.Social.Config;

internal sealed class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
	public void Configure(EntityTypeBuilder<Friendship> f)
	{
		f.ToTable("friendships", "social");
		f.HasKey(f => f.Id);

		f.Property(f => f.Id).HasColumnName("id").ValueGeneratedOnAdd();
		f.Property(f => f.UserId1).HasColumnName("user_id_1").IsRequired();
		f.Property(f => f.UserId2).HasColumnName("user_id_2").IsRequired();
		f.Property(f => f.CreatedAt).HasColumnName("created_at").IsRequired();
		
		f.HasIndex(f => new { f.UserId1, f.UserId2 }).IsUnique();
		f.ToTable(t => t.HasCheckConstraint("social_friendships_c_self_friendship", "user_id_1 <> user_id_2"));

		f.HasOne(f => f.User1)
			.WithMany()
			.HasForeignKey(f => f.UserId1)
			.HasConstraintName("social_friendships_fk_user_1")
			.OnDelete(DeleteBehavior.Cascade);
		
		f.HasOne(f => f.User2)
			.WithMany()
			.HasForeignKey(f => f.UserId2)
			.HasConstraintName("social_friendships_fk_user_2")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
