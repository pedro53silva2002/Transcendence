using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Config;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.ToTable("refresh_tokens", "auth");

        b.HasKey(r => r.Id);
        b.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();
        b.Property(r => r.UserId).HasColumnName("user_id").IsRequired();
        b.Property(r => r.Token).HasColumnName("token").IsRequired();
        b.Property(r => r.ExpiresAt).HasColumnName("expires_at").IsRequired();
        b.Property(r => r.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(r => r.RevokedAt).HasColumnName("revoked_at");

        b.HasIndex(r => r.Token).IsUnique();
    }
}
