using University_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusServices.Infrastructure.Persistence.Configurations;

public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lectures");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Title).IsRequired().HasMaxLength(200);
        builder.Property(l => l.Description).HasMaxLength(2000);
        builder.Property(l => l.OrderNumber).IsRequired();
        builder.Property(l => l.IsPublished).HasDefaultValue(false);
        builder.Property(l => l.CreatedAt).IsRequired();
        builder.Property(l => l.UpdatedAt);

        // Not unique on purpose, so reordering lectures doesn't hit a temporary clash.
        builder.HasIndex(l => new { l.CourseId, l.OrderNumber });

        builder.HasOne(l => l.Course)
            .WithMany(c => c.Lectures)
            .HasForeignKey(l => l.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class LectureMaterialConfiguration : IEntityTypeConfiguration<LectureMaterial>
{
    public void Configure(EntityTypeBuilder<LectureMaterial> builder)
    {
        builder.ToTable("LectureMaterials");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
        builder.Property(m => m.MaterialType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(m => m.FileName).HasMaxLength(255);
        builder.Property(m => m.FilePath).HasMaxLength(500);
        builder.Property(m => m.ContentType).HasMaxLength(100);
        builder.Property(m => m.Url).HasMaxLength(1000);
        builder.Property(m => m.CreatedAt).IsRequired();

        builder.HasIndex(m => m.LectureId);

        builder.HasOne(m => m.Lecture)
            .WithMany(l => l.Materials)
            .HasForeignKey(m => m.LectureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}