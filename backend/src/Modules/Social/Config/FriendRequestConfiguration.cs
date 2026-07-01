using NpgsqlTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;
using Trippie.Modules.Social.Model;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Social.Config;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FriendRequestStatus
{
	[PgName("pending")] Pending,
	//[PgName("accepted")] Accepted, still dont know if we need this, maybe we can just delete the request when accepted
}

internal sealed class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
	public void Configure(EntityTypeBuilder<FriendRequest> fr)
	{
		fr.ToTable("friend_requests", "social");
		fr.HasKey(fr => fr.Id);

		fr.Property(fr => fr.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd();

		fr.Property(fr => fr.SenderId)
			.HasColumnName("sender_id")
			.IsRequired();

		fr.Property(fr => fr.ReceiverId)
			.HasColumnName("receiver_id")
			.IsRequired();

		fr.Property(fr => fr.Status)
			.HasColumnName("status")
			.HasDefaultValue(FriendRequestStatus.Pending)
			.IsRequired();

		fr.Property(fr => fr.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("now()")
			.IsRequired();

		fr.Property(fr => fr.UpdatedAt)
			.HasColumnName("updated_at")
			.IsRequired(false);

		fr.HasOne<User>()
			.WithMany()
			.HasForeignKey(fr => fr.SenderId)
			.HasConstraintName("social_friend_requests_fk_sender")
			.OnDelete(DeleteBehavior.Cascade);

		fr.HasOne<User>()
			.WithMany()
			.HasForeignKey(fr => fr.ReceiverId)
			.HasConstraintName("social_friend_requests_fk_receiver")
			.OnDelete(DeleteBehavior.Cascade);

		fr.HasIndex(fr => new { fr.SenderId, fr.ReceiverId })
			.IsUnique()
			.HasDatabaseName("social_friend_requests_u_sender_receiver");

		fr.HasIndex(fr => fr.ReceiverId)
			.HasDatabaseName("social_friend_requests_i_receiver_pending")
			.HasFilter("status = 'pending'");
		
		fr.HasIndex(fr => fr.SenderId)
			.HasDatabaseName("social_friend_requests_i_sender_pending")
			.HasFilter("status = 'pending'");

		fr.ToTable(t => t.HasCheckConstraint(
			"social_friend_requests_c_self_request",
			"sender_id <> receiver_id"));
	}
}
