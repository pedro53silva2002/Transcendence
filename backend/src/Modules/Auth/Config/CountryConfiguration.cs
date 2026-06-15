using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Config;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> b)
    {
        b.ToTable("countries", "auth");

        b.HasKey(c => c.Id);
        b.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        b.Property(c => c.Name).HasColumnName("name").IsRequired();
        b.Property(c => c.Code).HasColumnName("code").HasMaxLength(2).IsFixedLength().IsRequired();

        b.HasIndex(c => c.Code).IsUnique();
    }
}
