
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University_Domain.Entities;

namespace CampusServices.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CourseCode).IsRequired().HasMaxLength(30);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).IsRequired();

        builder.HasIndex(c => c.CourseCode).IsUnique();
        builder.HasIndex(c => c.DoctorId);

        builder.HasOne(c => c.Doctor)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AttendanceSessionConfiguration : IEntityTypeConfiguration<AttendanceSession>
{
    public void Configure(EntityTypeBuilder<AttendanceSession> builder)
    {
        builder.ToTable("AttendanceSessions");
        builder.HasKey(s => s.Id);

        // Only the hash of the QR token is stored, never the raw token.
        builder.Property(s => s.QRTokenHash).IsRequired().HasMaxLength(128);
        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        builder.Property(s => s.IsOpen).HasDefaultValue(true);
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasIndex(s => s.QRTokenHash).IsUnique();
        builder.HasIndex(s => new { s.CourseId, s.StartTime });
        builder.HasIndex(s => s.DoctorId);

        builder.HasOne(s => s.Course)
            .WithMany(c => c.AttendanceSessions)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Doctor)
            .WithMany(d => d.AttendanceSessions)
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.AttendanceStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(r => r.RecordedAt).IsRequired();

        // A student cannot register twice in the same session.
        builder.HasIndex(r => new { r.AttendanceSessionId, r.StudentId }).IsUnique();
        builder.HasIndex(r => r.StudentId);

        builder.HasOne(r => r.AttendanceSession)
            .WithMany(s => s.Records)
            .HasForeignKey(r => r.AttendanceSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Student)
            .WithMany(s => s.AttendanceRecords)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}