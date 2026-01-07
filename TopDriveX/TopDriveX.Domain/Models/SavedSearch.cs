using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Domain.Models
{
    public class SavedSearch : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string SearchCriteria { get; set; } // JSON
        public bool EnableEmailNotifications { get; set; } = true;
        public int NotificationFrequencyHours { get; set; } = 24;
        public DateTime? LastNotifiedAt { get; set; }

        // Navigation
        public virtual User User { get; set; }
    }
}
