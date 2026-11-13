using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class AttendanceSession : BaseEntity
    {
        public int CourseId { get; set; }
        public int DoctorId { get; set; }

        /// <summary>Hash of the QR token only. The raw token is never stored.</summary>
        public string QRTokenHash { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOpen { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Course Course { get; set; } = null!;
        public DoctorProfile Doctor { get; set; } = null!;
        public ICollection<AttendanceRecord> Records { get; set; } = new List<AttendanceRecord>();
    }
}
