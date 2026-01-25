using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            });
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);

            if (vehicle == null)
                return null;

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
                Images = vehicle.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
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
            });
        }

        public async Task<IEnumerable<VehicleListDto>> GetFeaturedVehiclesAsync(int count = 6)
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            return vehicles.Take(count).Select(v => new VehicleListDto
            {
                Id = v.Id,
                MakeName = v.Make?.Name ?? "",
                ModelName = v.Model?.Name ?? "",
                Year = v.Year,
                Price = v.Price,
                Mileage = v.Mileage,
                City = v.City,
                MainImage = v.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
            });
        }

        public async Task<IEnumerable<VehicleListDto>> GetLatestVehiclesAsync(int count = 10)
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            return vehicles
                .OrderByDescending(v => v.CreatedAt)
                .Take(count)
                .Select(v => new VehicleListDto
                {
                    Id = v.Id,
                    MakeName = v.Make?.Name ?? "",
                    ModelName = v.Model?.Name ?? "",
                    Year = v.Year,
                    Price = v.Price,
                    Mileage = v.Mileage,
                    City = v.City,
                    MainImage = v.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                });
        }
    }
}
