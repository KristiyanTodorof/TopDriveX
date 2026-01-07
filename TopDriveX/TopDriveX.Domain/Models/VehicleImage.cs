using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Domain.Models
{
    public class VehicleImage : BaseEntity
    {
        public Guid VehicleId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
        public string? Caption { get; set; }

        // Navigation
        public virtual Vehicle Vehicle { get; set; }
    }
}
