using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class CreateVehicleDto
    {
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal Price { get; set; }
        public int FuelType { get; set; }
        public int TransmissionType { get; set; }
        public string City { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
