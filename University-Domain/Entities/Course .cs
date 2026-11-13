using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{

    public class Course : BaseEntity
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DoctorId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DoctorProfile Doctor { get; set; } = null!;
        public ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();
        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

        public ICollection<Assignment> Assignments { get; set; }= new List<Assignment>();

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }

}
