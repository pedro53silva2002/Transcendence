using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trippie.Modules.Travel.Model;

namespace Trippie.Modules.Travel.Config;

internal sealed class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> b)
    {
        // 1st argument is the table name, second one is the schema
        b.ToTable("trips", "travel");
        // Primary key
        b.HasKey(t => t.Id);

        b.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
        b.Property(t => t.TripName).HasColumnName("name").IsRequired();
        b.Property(t => t.Description).HasColumnName("description");
        b.Property(t => t.Duration).HasColumnName("duration").IsRequired().HasDefaultValue(0);
        b.Property(t => t.StartDate).HasColumnName("start_date").IsRequired();
        b.Property(t => t.EndDate).HasColumnName("end_date").IsRequired();
        b.Property(t => t.Budget).HasColumnName("budget").IsRequired().HasDefaultValue(0);
        b.Property(t => t.Visibility).HasColumnName("visibility").HasColumnType("trip_visibility").IsRequired();
        b.Property(t => t.CreatedBy).HasColumnName("created_by").IsRequired();
        b.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(t => t.UpdatedAt).HasColumnName("updated_at");
    }
}
