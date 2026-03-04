using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Application.Dtos
{
    public class EditAdvertisementDto
    {
        public Guid AdvertisementId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid OwnerId { get; set; }

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

        public string? VIN { get; set; }

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

        [Required(ErrorMessage = "Моля въведете заглавие")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Между 10 и 150 символа")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля въведете описание")]
        [StringLength(5000, MinimumLength = 30, ErrorMessage = "Между 30 и 5000 символа")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля въведете цена")]
        [Range(1, 10000000, ErrorMessage = "Цената трябва да е между 1 и 10,000,000")]
        public decimal Price { get; set; }

        public bool IsNegotiable { get; set; }

        [Required(ErrorMessage = "Моля въведете град")]
        public string City { get; set; } = string.Empty;

        public string? Region { get; set; }
        public List<string> ExistingImages { get; set; } = new();
        public string ImagesToDeleteJson { get; set; } = "[]";
        public List<IFormFile>? NewImages { get; set; }

        public string? Features { get; set; }
    }
}
