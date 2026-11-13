using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class QuizAnswer : BaseEntity
    {
        public int QuizAttemptId { get; set; }
        public int QuizQuestionId { get; set; }

        public int? SelectedOptionId { get; set; }

        public string? TextAnswer { get; set; }

        public bool? IsCorrect { get; set; }

        public decimal? AwardedPoints { get; set; }

        public QuizAttempt QuizAttempt { get; set; } = null!;

        public QuizQuestion QuizQuestion { get; set; } = null!;

        public QuizOption? SelectedOption { get; set; }

        public ICollection<QuizAnswerAttachment> Attachments { get; set; }
            = new List<QuizAnswerAttachment>();
    }
}
