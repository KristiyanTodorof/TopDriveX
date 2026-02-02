using Microsoft.EntityFrameworkCore;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;
using TopDriveX.Domain.Models;

namespace TopDriveX.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleListDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            // Load related data
            foreach (var vehicle in vehicles)
            {
                await LoadVehicleRelatedDataAsync(vehicle);
            }

            return vehicles.Select(v => new VehicleListDto
            {
                Id = v.Id,
                MakeName = v.Make?.Name ?? "",
                ModelName = v.Model?.Name ?? "",
                Year = v.Year,
                Price = v.Price,
                Mileage = v.Mileage,
                City = v.City,
                MainImage = v.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? v.Images?.FirstOrDefault()?.ImageUrl
            });
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);

            if (vehicle == null)
                return null;

            await LoadVehicleRelatedDataAsync(vehicle);

            return new VehicleDto
            {
                Id = vehicle.Id,
                MakeName = vehicle.Make?.Name ?? "",
                ModelName = vehicle.Model?.Name ?? "",
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                Price = vehicle.Price,
                FuelType = vehicle.FuelType.ToString(),
                TransmissionType = vehicle.TransmissionType.ToString(),
                Color = vehicle.Color,
                City = vehicle.City,
                Images = vehicle.Images?.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList() ?? new List<string>()
            };
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto)
        {
            var vehicle = new Vehicle
            {
                MakeId = dto.MakeId,
                ModelId = dto.ModelId,
                Year = dto.Year,
                Mileage = dto.Mileage,
                Price = dto.Price,
                FuelType = (Domain.Enums.FuelType)dto.FuelType,
                TransmissionType = (Domain.Enums.TransmissionType)dto.TransmissionType,
                City = dto.City,
                Description = dto.Description,
                Condition = Domain.Enums.VehicleCondition.Used
            };

            await _unitOfWork.Vehicles.AddAsync(vehicle);
            await _unitOfWork.SaveChangesAsync();

            return (await GetVehicleByIdAsync(vehicle.Id))!;
        }

        public async Task<VehicleDto?> UpdateVehicleAsync(Guid id, CreateVehicleDto dto)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null)
                return null;

            vehicle.MakeId = dto.MakeId;
            vehicle.ModelId = dto.ModelId;
            vehicle.Year = dto.Year;
            vehicle.Mileage = dto.Mileage;
            vehicle.Price = dto.Price;
            vehicle.FuelType = (Domain.Enums.FuelType)dto.FuelType;
            vehicle.TransmissionType = (Domain.Enums.TransmissionType)dto.TransmissionType;
            vehicle.City = dto.City;
            vehicle.Description = dto.Description;

            _unitOfWork.Vehicles.Update(vehicle);
            await _unitOfWork.SaveChangesAsync();

            return await GetVehicleByIdAsync(id);
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null)
                return false;

            _unitOfWork.Vehicles.Remove(vehicle);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<VehicleListDto>> SearchVehiclesAsync(
            Guid? makeId = null,
            Guid? modelId = null,
            int? yearFrom = null,
            int? yearTo = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            int? mileageFrom = null,
            int? mileageTo = null,
            string? city = null)
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            // Load related data
            foreach (var vehicle in vehicles)
            {
                await LoadVehicleRelatedDataAsync(vehicle);
            }

            var filtered = vehicles.AsEnumerable();

            if (makeId.HasValue)
                filtered = filtered.Where(v => v.MakeId == makeId.Value);

            if (modelId.HasValue)
                filtered = filtered.Where(v => v.ModelId == modelId.Value);

            if (yearFrom.HasValue)
                filtered = filtered.Where(v => v.Year >= yearFrom.Value);

            if (yearTo.HasValue)
                filtered = filtered.Where(v => v.Year <= yearTo.Value);

            if (priceFrom.HasValue)
                filtered = filtered.Where(v => v.Price >= priceFrom.Value);

            if (priceTo.HasValue)
                filtered = filtered.Where(v => v.Price <= priceTo.Value);

            if (mileageFrom.HasValue)
                filtered = filtered.Where(v => v.Mileage >= mileageFrom.Value);

            if (mileageTo.HasValue)
                filtered = filtered.Where(v => v.Mileage <= mileageTo.Value);

            if (!string.IsNullOrWhiteSpace(city))
                filtered = filtered.Where(v => v.City.Contains(city));

            return filtered.Select(v => new VehicleListDto
            {
                Id = v.Id,
                MakeName = v.Make?.Name ?? "",
                ModelName = v.Model?.Name ?? "",
                Year = v.Year,
                Price = v.Price,
                Mileage = v.Mileage,
                City = v.City,
                MainImage = v.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? v.Images?.FirstOrDefault()?.ImageUrl
            });
        }

        public async Task<IEnumerable<VehicleListDto>> GetFeaturedVehiclesAsync(int count = 6)
        {
            var vehicles = (await _unitOfWork.Vehicles.GetAllAsync()).Take(count);

            foreach (var vehicle in vehicles)
            {
                await LoadVehicleRelatedDataAsync(vehicle);
            }

            return vehicles.Select(v => new VehicleListDto
            {
                Id = v.Id,
                MakeName = v.Make?.Name ?? "",
                ModelName = v.Model?.Name ?? "",
                Year = v.Year,
                Price = v.Price,
                Mileage = v.Mileage,
                City = v.City,
                MainImage = v.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? v.Images?.FirstOrDefault()?.ImageUrl
            });
        }

        public async Task<IEnumerable<VehicleListDto>> GetLatestVehiclesAsync(int count = 10)
        {
            var vehicles = (await _unitOfWork.Vehicles.GetAllAsync())
                .OrderByDescending(v => v.CreatedAt)
                .Take(count);

            foreach (var vehicle in vehicles)
            {
                await LoadVehicleRelatedDataAsync(vehicle);
            }

            return vehicles.Select(v => new VehicleListDto
            {
                Id = v.Id,
                MakeName = v.Make?.Name ?? "",
                ModelName = v.Model?.Name ?? "",
                Year = v.Year,
                Price = v.Price,
                Mileage = v.Mileage,
                City = v.City,
                MainImage = v.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                    ?? v.Images?.FirstOrDefault()?.ImageUrl
            });
        }

        // Helper method to load related navigation properties
        private async Task LoadVehicleRelatedDataAsync(Vehicle vehicle)
        {
            if (vehicle.Make == null)
            {
                vehicle.Make = await _unitOfWork.Makes.GetByIdAsync(vehicle.MakeId);
            }

            if (vehicle.Model == null)
            {
                vehicle.Model = await _unitOfWork.Models.GetByIdAsync(vehicle.ModelId);
            }

            if (vehicle.Images == null || !vehicle.Images.Any())
            {
                var allImages = await _unitOfWork.VehicleImages.FindAsync(i => i.VehicleId == vehicle.Id);
                vehicle.Images = allImages.OrderBy(i => i.DisplayOrder).ToList();
            }
        }

        public async Task<VehicleDetailsDto?> GetVehicleDetailsAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);

            if (vehicle == null)
                return null;

            await LoadVehicleRelatedDataAsync(vehicle);

            return new VehicleDetailsDto
            {
                Id = vehicle.Id,
                MakeName = vehicle.Make?.Name ?? "",
                ModelName = vehicle.Model?.Name ?? "",
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                Price = vehicle.Price,
                FuelType = vehicle.FuelType.ToString(),
                TransmissionType = vehicle.TransmissionType.ToString(),
                BodyStyle = vehicle.BodyStyle?.ToString(),
                Condition = vehicle.Condition.ToString(),
                EngineSize = vehicle.EngineSize,
                HorsePower = vehicle.HorsePower,
                Cylinders = vehicle.Cylinders,
                Color = vehicle.Color,
                InteriorColor = vehicle.InteriorColor,
                Doors = vehicle.Doors,
                Seats = vehicle.Seats,
                City = vehicle.City,
                Region = vehicle.Region,
                Country = vehicle.Country,
                Description = vehicle.Description,
                VIN = vehicle.VIN,
                Images = vehicle.Images?.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                CreatedAt = vehicle.CreatedAt
            };
        }
    }
}