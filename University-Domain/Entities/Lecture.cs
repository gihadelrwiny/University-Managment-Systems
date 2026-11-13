using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class Lecture : BaseEntity
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int OrderNumber { get; set; }

        public bool IsPublished { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Course Course { get; set; } = null!;

        public ICollection<LectureMaterial> Materials { get; set; }
            = new List<LectureMaterial>();
    }
}
