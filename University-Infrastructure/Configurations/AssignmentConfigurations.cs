using University_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusServices.Infrastructure.Persistence.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments", t =>
        {
            t.HasCheckConstraint("CK_Assignments_MaxGrade", "\"MaxGrade\" > 0");
        });

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(4000);
        builder.Property(a => a.MaxGrade).HasPrecision(6, 2).IsRequired().HasDefaultValue(100m);
        builder.Property(a => a.DueDate).IsRequired();
        builder.Property(a => a.IsPublished).HasDefaultValue(false);
        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.UpdatedAt);

        builder.HasIndex(a => new { a.CourseId, a.DueDate });
        builder.HasIndex(a => new { a.CourseId, a.IsPublished });

        builder.HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AssignmentSubmissionConfiguration : IEntityTypeConfiguration<AssignmentSubmission>
{
    public void Configure(EntityTypeBuilder<AssignmentSubmission> builder)
    {
        builder.ToTable("AssignmentSubmissions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TextAnswer).HasColumnType("text");
        builder.Property(s => s.SubmittedAt).IsRequired();

        // One submission per student per assignment.
        builder.HasIndex(s => new { s.AssignmentId, s.StudentId }).IsUnique();
        builder.HasIndex(s => s.StudentId);

        builder.HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one: the grade holds the FK.
        builder.HasOne(s => s.Grade)
            .WithOne(g => g.AssignmentSubmission)
            .HasForeignKey<AssignmentGrade>(g => g.AssignmentSubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AssignmentAttachmentConfiguration : IEntityTypeConfiguration<AssignmentAttachment>
{
    public void Configure(EntityTypeBuilder<AssignmentAttachment> builder)
    {
        builder.ToTable("AssignmentAttachments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName).IsRequired().HasMaxLength(255);
        builder.Property(a => a.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(a => a.UploadedAt).IsRequired();

        builder.HasIndex(a => a.AssignmentSubmissionId);

        builder.HasOne(a => a.AssignmentSubmission)
            .WithMany(s => s.Attachments)
            .HasForeignKey(a => a.AssignmentSubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AssignmentGradeConfiguration : IEntityTypeConfiguration<AssignmentGrade>
{
    public void Configure(EntityTypeBuilder<AssignmentGrade> builder)
    {
        builder.ToTable("AssignmentGrades", t =>
        {
            t.HasCheckConstraint("CK_AssignmentGrades_Score", "\"Score\" >= 0");
        });

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Score).HasPrecision(6, 2).IsRequired();
        builder.Property(g => g.Feedback).HasMaxLength(4000);
        builder.Property(g => g.GradedAt).IsRequired();

        // One grade per submission.
        builder.HasIndex(g => g.AssignmentSubmissionId).IsUnique();
        builder.HasIndex(g => g.GradedByUserId);

        builder.HasOne(g => g.GradedByUser)
            .WithMany()
            .HasForeignKey(g => g.GradedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}