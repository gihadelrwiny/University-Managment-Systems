using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class DoctorProfile : BaseEntity
    {
        public int UserId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string AcademicTitle { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();
    }
}
