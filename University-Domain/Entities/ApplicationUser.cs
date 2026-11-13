using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public StudentProfile? StudentProfile { get; set; }
        public DoctorProfile? DoctorProfile { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
