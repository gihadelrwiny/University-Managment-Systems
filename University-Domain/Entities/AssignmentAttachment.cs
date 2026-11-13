using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class AssignmentAttachment : BaseEntity
    {
        public int AssignmentSubmissionId { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public AssignmentSubmission AssignmentSubmission { get; set; } = null!;
    }
}
