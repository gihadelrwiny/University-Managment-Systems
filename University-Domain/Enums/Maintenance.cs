using System;
using System.Collections.Generic;
using System.Text;

namespace University_Domain.Enums
{
    public enum MaintenanceCategory
    {
        Electrical = 1,
        Plumbing = 2,
        Furniture = 3,
        Cleaning = 4,
        GeneralMaintenance = 5,
        Other = 6
    }

    public enum MaintenanceStatus
    {
        Pending = 1,
        InProgress = 2,
        Resolved = 3,
        Rejected = 4
    }
}
