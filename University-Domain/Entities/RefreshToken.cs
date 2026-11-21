using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresOnUtc { get; set; }

        public DateTime? RevokedOnUtc { get; set; }

        public bool IsRevoked => RevokedOnUtc != null;

        public bool IsExpired => ExpiresOnUtc <= DateTime.UtcNow;

        public bool IsActive => !IsRevoked && !IsExpired;

        public ApplicationUser User { get; set; } = null!;
    }
}
