using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{
    public class MaintenanceRequest : BaseEntity
    {
        public int StudentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public MaintenanceCategory Category { get; set; }
        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;
        public string? AdminComment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public StudentProfile Student { get; set; } = null!;
        public ICollection<MaintenanceAttachment> Attachments { get; set; } = new List<MaintenanceAttachment>();
        public ICollection<MaintenanceStatusHistory> StatusHistory { get; set; } = new List<MaintenanceStatusHistory>();
    }
}
