using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class AssignmentGrade : BaseEntity
    {
        public int AssignmentSubmissionId { get; set; }

        public int GradedByUserId { get; set; }

        public decimal Score { get; set; }

        public string? Feedback { get; set; }

        public DateTime GradedAt { get; set; } = DateTime.UtcNow;

        public AssignmentSubmission AssignmentSubmission { get; set; } = null!;

        public ApplicationUser GradedByUser { get; set; } = null!;
    }
}
