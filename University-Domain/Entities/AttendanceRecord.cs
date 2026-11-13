using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{

    public class AttendanceRecord : BaseEntity
    {
        public int AttendanceSessionId { get; set; }
        public int StudentId { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; } = AttendanceStatus.Present;
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        public AttendanceSession AttendanceSession { get; set; } = null!;
        public StudentProfile Student { get; set; } = null!;
    }
}
