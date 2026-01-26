using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Domain.Models
{
    public class Make : BaseEntity
    {
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public string? Country { get; set; }

        // Navigation
        public virtual ICollection<Model> Models { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
