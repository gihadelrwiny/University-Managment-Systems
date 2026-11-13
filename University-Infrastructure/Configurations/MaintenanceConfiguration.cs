
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Domain.Entities;

namespace CampusServices.Infrastructure.Persistence.Configurations;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
    {
        builder.ToTable("MaintenanceRequests");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Description).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.Category).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(m => m.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(m => m.AdminComment).HasMaxLength(2000);
        builder.Property(m => m.CreatedAt).IsRequired();

        builder.HasIndex(m => m.StudentId);
        builder.HasIndex(m => new { m.Status, m.Category });
        builder.HasIndex(m => m.CreatedAt);

        builder.HasOne(m => m.Student)
            .WithMany(s => s.MaintenanceRequests)
            .HasForeignKey(m => m.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MaintenanceAttachmentConfiguration : IEntityTypeConfiguration<MaintenanceAttachment>
{
    public void Configure(EntityTypeBuilder<MaintenanceAttachment> builder)
    {
        builder.ToTable("MaintenanceAttachments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName).IsRequired().HasMaxLength(255);
        builder.Property(a => a.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(a => a.UploadedAt).IsRequired();

        builder.HasIndex(a => a.MaintenanceRequestId);

        builder.HasOne(a => a.MaintenanceRequest)
            .WithMany(m => m.Attachments)
            .HasForeignKey(a => a.MaintenanceRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MaintenanceStatusHistoryConfiguration : IEntityTypeConfiguration<MaintenanceStatusHistory>
{
    public void Configure(EntityTypeBuilder<MaintenanceStatusHistory> builder)
    {
        builder.ToTable("MaintenanceStatusHistories");
        builder.HasKey(h => h.Id);

        builder.Property(h => h.PreviousStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(h => h.NewStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(h => h.Comment).HasMaxLength(2000);
        builder.Property(h => h.ChangedAt).IsRequired();

        builder.HasIndex(h => new { h.MaintenanceRequestId, h.ChangedAt });

        builder.HasOne(h => h.MaintenanceRequest)
            .WithMany(m => m.StatusHistory)
            .HasForeignKey(h => h.MaintenanceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}