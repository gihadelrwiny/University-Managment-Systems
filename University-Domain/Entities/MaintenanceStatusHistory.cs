using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{
    public class MaintenanceStatusHistory : BaseEntity
    {
        public int MaintenanceRequestId { get; set; }
        public int ChangedByUserId { get; set; }
        public MaintenanceStatus PreviousStatus { get; set; }
        public MaintenanceStatus NewStatus { get; set; }
        public string? Comment { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public MaintenanceRequest MaintenanceRequest { get; set; } = null!;
        public ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
