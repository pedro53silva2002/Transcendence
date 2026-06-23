using NpgsqlTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;
using Trippie.Modules.Social.Model;

namespace Trippie.Modules.Social.Config;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FriendRequestStatus
{
	[PgName("pending")] Pending,
	[PgName("accepted")] Accepted,
}

internal sealed class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
	public void Configure(EntityTypeBuilder<FriendRequest> fr)
	{
		fr.ToTable("friend_requests", "social");
		fr.HasKey(fr => fr.Id);

		fr.Property(fr => fr.Id).HasColumnName("id").ValueGeneratedOnAdd();
		fr.Property(fr => fr.SenderId).HasColumnName("sender_id").IsRequired();
		fr.Property(fr => fr.ReceiverId).HasColumnName("receiver_id").IsRequired();
		fr.Property(fr => fr.Status).HasColumnName("status").HasConversion<string>().IsRequired();
		fr.Property(fr => fr.CreatedAt).HasColumnName("created_at").IsRequired();
		fr.Property(fr => fr.UpdatedAt).HasColumnName("updated_at").IsRequired();

		fr.HasIndex(fr => new { fr.SenderId, fr.ReceiverId }).IsUnique();
		fr.HasIndex(fr => fr.ReceiverId).HasDatabaseName("social_friend_requests_i_receiver").HasFilter("status = 'pending'");

		fr.HasMany(fr => fr.SenderId)
			.WithMany()
			.HasForeignKey(fr => fr.SenderId)
			.HasConstraintName("social_friend_requests_sender")
			.OnDelete(DeleteBehavior.Cascade);

		fr.HasMany(fr => fr.ReceiverId)
			.WithMany()
			.HasForeignKey(fr => fr.ReceiverId)
			.HasConstraintName("social_friend_requests_receiver")
			.OnDelete(DeleteBehavior.Cascade);
		fr.ToTable(t => t.HasCheckConstraint("social_friend_requests_c_self_request", "sender_id <> receiver_id"));
	}
}
