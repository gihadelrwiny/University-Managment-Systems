using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{

    public class QuizAttempt : BaseEntity
    {
        public int QuizId { get; set; }
        public int StudentId { get; set; }

        public int AttemptNumber { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? SubmittedAt { get; set; }

        public decimal? Score { get; set; }

        public QuizAttemptStatus Status { get; set; }
            = QuizAttemptStatus.InProgress;

        public int? GradedByUserId { get; set; }
        public DateTime? GradedAt { get; set; }

        public Quiz Quiz { get; set; } = null!;

        public StudentProfile Student { get; set; } = null!;

        public ApplicationUser? GradedByUser { get; set; }

        public ICollection<QuizAnswer> Answers { get; set; }
            = new List<QuizAnswer>();
    }
}
