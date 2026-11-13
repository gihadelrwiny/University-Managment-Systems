using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class Quiz : BaseEntity
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public int MaxAttempts { get; set; } = 1;

        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }

        public bool IsPublished { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Course Course { get; set; } = null!;

        public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

        public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
