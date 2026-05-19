using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Config;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        // 1st argument is the table name, second one is the schema
        b.ToTable("users", "auth");
        // Primary key
        b.HasKey(u => u.Id);

        b.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
        b.Property(u => u.Email).HasColumnName("email").IsRequired();
        b.Property(u => u.Username).HasColumnName("username").IsRequired();
        b.Property(u => u.PasswordHash).HasColumnName("password_hash");
        b.Property(u => u.DisplayName).HasColumnName("display_name").IsRequired();
        b.Property(u => u.Bio).HasColumnName("bio");
        b.Property(u => u.ProfilePhotoUrl).HasColumnName("profile_photo_url");
        b.Property(u => u.OauthProvider).HasColumnName("oauth_provider");
        b.Property(u => u.OauthId).HasColumnName("oauth_id");
        b.Property(u => u.CreatedAt).HasColumnName("created_at");
        b.Property(u => u.UpdatedAt).HasColumnName("updated_at");

        b.HasIndex(u => u.Email).IsUnique();
        b.HasIndex(u => u.Username).IsUnique();
    }
}
