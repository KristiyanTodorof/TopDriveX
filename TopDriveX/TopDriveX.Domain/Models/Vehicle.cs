using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Domain.Models
{
    public class Vehicle : BaseEntity
    {
        // Foreign Keys
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }
        public Guid? VehicleTypeId { get; set; }

        // Basic Info
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal Price { get; set; }
        public string? VIN { get; set; }

        // Technical Specs
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public BodyStyle? BodyStyle { get; set; }
        public VehicleCondition Condition { get; set; }
        public decimal? EngineSize { get; set; }
        public int? HorsePower { get; set; }
        public int? Cylinders { get; set; }

        // Appearance
        public string? Color { get; set; }
        public string? InteriorColor { get; set; }
        public int? Doors { get; set; }
        public int? Seats { get; set; }

        // Location
        public string City { get; set; }
        public string? Region { get; set; }
        public string Country { get; set; } = "Bulgaria";

        // Description
        public string? Description { get; set; }
        public string? Features { get; set; } // JSON

        // Navigation
        public virtual Make Make { get; set; }
        public virtual Model Model { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
        public virtual ICollection<VehicleImage> Images { get; set; }
        public virtual Advertisement? Advertisement { get; set; }
    }
}
