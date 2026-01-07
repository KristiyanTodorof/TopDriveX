using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Domain.Models
{
    public class VehicleType : BaseEntity
    {
        public string Name { get; set; }
        public int? NhtsaVehicleTypeId { get; set; }
        public string? Description { get; set; }

        // Navigation
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
