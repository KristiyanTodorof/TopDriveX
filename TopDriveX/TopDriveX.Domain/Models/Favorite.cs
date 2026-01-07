using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Domain.Models
{
    public class Favorite : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AdvertisementId { get; set; }
        public string? Notes { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}
