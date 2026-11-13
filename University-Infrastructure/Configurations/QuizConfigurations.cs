using University_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusServices.Infrastructure.Persistence.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes", t =>
        {
            t.HasCheckConstraint("CK_Quizzes_Duration", "\"DurationMinutes\" > 0");
            t.HasCheckConstraint("CK_Quizzes_MaxAttempts", "\"MaxAttempts\" > 0");
            t.HasCheckConstraint("CK_Quizzes_Window",
                "\"AvailableFrom\" IS NULL OR \"AvailableUntil\" IS NULL OR \"AvailableUntil\" > \"AvailableFrom\"");
        });

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Title).IsRequired().HasMaxLength(200);
        builder.Property(q => q.Description).IsRequired().HasMaxLength(2000);
        builder.Property(q => q.DurationMinutes).IsRequired();
        builder.Property(q => q.MaxAttempts).IsRequired().HasDefaultValue(1);
        builder.Property(q => q.AvailableFrom);
        builder.Property(q => q.AvailableUntil);
        builder.Property(q => q.IsPublished).HasDefaultValue(false);
        builder.Property(q => q.CreatedAt).IsRequired();

        builder.HasIndex(q => new { q.CourseId, q.IsPublished });

        builder.HasOne(q => q.Course)
            .WithMany(c => c.Quizzes)
            .HasForeignKey(q => q.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.ToTable("QuizQuestions", t =>
        {
            t.HasCheckConstraint("CK_QuizQuestions_Points", "\"Points\" > 0");
        });

        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuestionText).IsRequired().HasMaxLength(2000);
        builder.Property(q => q.QuestionType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(q => q.Points).HasPrecision(6, 2).IsRequired();
        builder.Property(q => q.OrderNumber).IsRequired();

        builder.HasIndex(q => new { q.QuizId, q.OrderNumber });

        builder.HasOne(q => q.Quiz)
            .WithMany(z => z.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOption>
{
    public void Configure(EntityTypeBuilder<QuizOption> builder)
    {
        builder.ToTable("QuizOptions");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OptionText).IsRequired().HasMaxLength(1000);
        builder.Property(o => o.IsCorrect).HasDefaultValue(false);
        builder.Property(o => o.OrderNumber).IsRequired();

        builder.HasIndex(o => new { o.QuizQuestionId, o.OrderNumber });

        builder.HasOne(o => o.QuizQuestion)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuizQuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.ToTable("QuizAttempts", t =>
        {
            t.HasCheckConstraint("CK_QuizAttempts_AttemptNumber", "\"AttemptNumber\" > 0");
        });

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AttemptNumber).IsRequired();
        builder.Property(a => a.StartedAt).IsRequired();
        builder.Property(a => a.SubmittedAt);
        builder.Property(a => a.Score).HasPrecision(7, 2);
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(a => a.GradedByUserId);
        builder.Property(a => a.GradedAt);

        // Attempt numbers are unique per student per quiz (MaxAttempts is enforced in the service layer).
        builder.HasIndex(a => new { a.QuizId, a.StudentId, a.AttemptNumber }).IsUnique();
        builder.HasIndex(a => a.StudentId);

        builder.HasOne(a => a.Quiz)
            .WithMany(q => q.Attempts)
            .HasForeignKey(a => a.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.GradedByUser)
            .WithMany()
            .HasForeignKey(a => a.GradedByUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAnswer> builder)
    {
        builder.ToTable("QuizAnswers");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.TextAnswer).HasColumnType("text");
        builder.Property(a => a.IsCorrect);
        builder.Property(a => a.AwardedPoints).HasPrecision(6, 2);

        // One answer per question per attempt.
        builder.HasIndex(a => new { a.QuizAttemptId, a.QuizQuestionId }).IsUnique();
        builder.HasIndex(a => a.QuizQuestionId);

        builder.HasOne(a => a.QuizAttempt)
            .WithMany(t => t.Answers)
            .HasForeignKey(a => a.QuizAttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.QuizQuestion)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuizQuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.SelectedOption)
            .WithMany(o => o.SelectedAnswers)
            .HasForeignKey(a => a.SelectedOptionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class QuizAnswerAttachmentConfiguration : IEntityTypeConfiguration<QuizAnswerAttachment>
{
    public void Configure(EntityTypeBuilder<QuizAnswerAttachment> builder)
    {
        builder.ToTable("QuizAnswerAttachments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName).IsRequired().HasMaxLength(255);
        builder.Property(a => a.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(a => a.UploadedAt).IsRequired();

        builder.HasIndex(a => a.QuizAnswerId);

        builder.HasOne(a => a.QuizAnswer)
            .WithMany(q => q.Attachments)
            .HasForeignKey(a => a.QuizAnswerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}