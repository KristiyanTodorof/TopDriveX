using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class VehicleDetailsDto
    {
        // Basic Info
        public Guid Id { get; set; }
        public string MakeName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal Price { get; set; }

        // Technical Details
        public string FuelType { get; set; } = string.Empty;
        public string TransmissionType { get; set; } = string.Empty;
        public string? BodyStyle { get; set; }
        public string Condition { get; set; } = string.Empty;
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
        public string Country { get; set; } = "Bulgaria";

        // Description
        public string? Description { get; set; }
        public string? VIN { get; set; }

        // Images
        public List<string> Images { get; set; } = new();

        // Dates
        public DateTime CreatedAt { get; set; }
    }
}
