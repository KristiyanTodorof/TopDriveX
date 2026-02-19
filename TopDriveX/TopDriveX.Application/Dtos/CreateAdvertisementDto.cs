using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Application.Dtos
{
    public class CreateAdvertisementDto
    {
        [Required(ErrorMessage = "Моля изберете марка")]
        public Guid MakeId { get; set; }

        [Required(ErrorMessage = "Моля изберете модел")]
        public Guid ModelId { get; set; }

        public Guid? VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Моля въведете година")]
        [Range(1900, 2030, ErrorMessage = "Невалидна година")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Моля въведете пробег")]
        [Range(0, 2000000, ErrorMessage = "Невалиден пробег")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Моля изберете гориво")]
        public FuelType FuelType { get; set; }

        [Required(ErrorMessage = "Моля изберете скоростна кутия")]
        public TransmissionType TransmissionType { get; set; }

        public BodyStyle? BodyStyle { get; set; }

        [Required(ErrorMessage = "Моля изберете състояние")]
        public VehicleCondition Condition { get; set; }

        [Range(1, 5000)]
        public int? HorsePower { get; set; }

        [Range(0, 100)]
        public decimal? EngineSize { get; set; }

        public int? Cylinders { get; set; }
        public string? Color { get; set; }
        public string? InteriorColor { get; set; }

        [Range(2, 6)]
        public int? Doors { get; set; }

        [Range(1, 9)]
        public int? Seats { get; set; }

        public string? VIN { get; set; }

        // ==================== STEP 2: FEATURES ====================

        public bool HasAirConditioner { get; set; }
        public bool HasNavigation { get; set; }
        public bool HasLeatherSeats { get; set; }
        public bool HasParkingSensors { get; set; }
        public bool HasRearCamera { get; set; }
        public bool HasHeatedSeats { get; set; }
        public bool HasSunroof { get; set; }
        public bool HasAlloyWheels { get; set; }
        public bool HasBluetooth { get; set; }
        public bool HasCruiseControl { get; set; }
        public bool HasXenon { get; set; }
        public bool HasLED { get; set; }
        public bool HasKeylessEntry { get; set; }
        public bool HasStartStop { get; set; }

        // ==================== STEP 3: AD DETAILS ====================

        [Required(ErrorMessage = "Моля въведете заглавие")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Между 10 и 150 символа")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля въведете описание")]
        [StringLength(5000, MinimumLength = 30, ErrorMessage = "Между 30 и 5000 символа")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля въведете цена")]
        [Range(0, 10000000)]
        public decimal Price { get; set; }

        public bool IsNegotiable { get; set; }

        [Required(ErrorMessage = "Моля въведете град")]
        public string City { get; set; } = string.Empty;

        public string? Region { get; set; }
        public string? ContactPhone { get; set; }

        // ==================== STEP 4: IMAGES ====================

        public List<IFormFile>? Images { get; set; }

        // ==================== MAPPING ====================

        public CreateVehicleDto ToDto()
        {
            // Build features list and serialize to JSON
            var features = new Dictionary<string, bool>
            {
                ["Климатик"] = HasAirConditioner,
                ["Навигация"] = HasNavigation,
                ["Кожен салон"] = HasLeatherSeats,
                ["Парктроник"] = HasParkingSensors,
                ["Камера"] = HasRearCamera,
                ["Подгряване"] = HasHeatedSeats,
                ["Люк"] = HasSunroof,
                ["Ал. джанти"] = HasAlloyWheels,
                ["Bluetooth"] = HasBluetooth,
                ["Темпомат"] = HasCruiseControl,
                ["Ксенон"] = HasXenon,
                ["LED"] = HasLED,
                ["Keyless"] = HasKeylessEntry,
                ["Start/Stop"] = HasStartStop,
            };

            var active = features.Where(f => f.Value).Select(f => f.Key).ToList();

            return new CreateVehicleDto
            {
                // Vehicle
                MakeId = MakeId,
                ModelId = ModelId,
                VehicleTypeId = VehicleTypeId,
                Year = Year,
                Mileage = Mileage,
                VIN = VIN,
                FuelType = FuelType,
                TransmissionType = TransmissionType,
                BodyStyle = BodyStyle,
                Condition = Condition,
                EngineSize = EngineSize,
                HorsePower = HorsePower,
                Cylinders = Cylinders,
                Color = Color,
                InteriorColor = InteriorColor,
                Doors = Doors,
                Seats = Seats,
                City = City,
                Region = Region,
                Description = Description,
                Features = active.Any() ? JsonSerializer.Serialize(active) : null,
                // Advertisement
                Title = Title,
                Price = Price,
                IsNegotiable = IsNegotiable,
                ContactPhone = ContactPhone,
                // Images
                Images = Images,
            };
        }
    }
}
