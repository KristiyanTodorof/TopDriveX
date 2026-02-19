using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Application.Dtos
{
    public class CreateVehicleDto
    {
        // Vehicle identification
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }
        public Guid? VehicleTypeId { get; set; }

        // Basic info
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string? VIN { get; set; }

        // Technical specs
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
        public string City { get; set; } = string.Empty;
        public string? Region { get; set; }

        // Description & features (JSON)
        public string? Description { get; set; }
        public string? Features { get; set; }

        // Advertisement fields
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsNegotiable { get; set; }
        public string? ContactPhone { get; set; }

        // Images
        public List<IFormFile>? Images { get; set; }
    }
}
