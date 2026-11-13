using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class StudentProfile : BaseEntity
    {
        public int UserId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int AcademicYear { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
    }

}
