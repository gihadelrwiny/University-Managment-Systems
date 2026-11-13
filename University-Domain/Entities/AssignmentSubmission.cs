using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class AssignmentSubmission : BaseEntity
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }

        public string? TextAnswer { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public Assignment Assignment { get; set; } = null!;
        public StudentProfile Student { get; set; } = null!;

        public ICollection<AssignmentAttachment> Attachments { get; set; }
            = new List<AssignmentAttachment>();

        public AssignmentGrade? Grade { get; set; }
    }
}
