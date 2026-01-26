using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Domain.Models
{
    public class Model : BaseEntity
    {
        public Guid MakeId { get; set; }
        public string Name { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        // Navigation
        public virtual Make Make { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
