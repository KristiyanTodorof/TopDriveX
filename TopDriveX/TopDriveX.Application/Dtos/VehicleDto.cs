using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string MakeName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal Price { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public string TransmissionType { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string City { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new();
    }
}
