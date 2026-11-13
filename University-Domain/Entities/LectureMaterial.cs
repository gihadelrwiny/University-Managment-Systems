using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{
    public class LectureMaterial : BaseEntity
    {
        public int LectureId { get; set; }

        public string Title { get; set; } = string.Empty;

        public LectureMaterialType MaterialType { get; set; }

        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }

        public string? Url { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Lecture Lecture { get; set; } = null!;
    }
}
