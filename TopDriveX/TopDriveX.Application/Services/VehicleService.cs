using Microsoft.EntityFrameworkCore;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;
using TopDriveX.Domain.Models;

namespace TopDriveX.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public VehicleService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        // ==================== GET ALL ====================

        public async Task<IEnumerable<VehicleListDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            foreach (var v in vehicles)
                await LoadVehicleRelatedDataAsync(v);

            return vehicles.Select(MapToListDto);
        }

        // ==================== GET BY ID ====================

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null) return null;

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
                Images = vehicle.Images?
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageUrl)
                    .ToList() ?? new List<string>()
            };
        }

        // ==================== CREATE VEHICLE ====================

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

        // ==================== CREATE ADVERTISEMENT ====================

        public async Task<Guid> CreateAdvertisementAsync(CreateVehicleDto dto, Guid userId)
        {
            // 1. Create Vehicle
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                MakeId = dto.MakeId,
                ModelId = dto.ModelId,
                VehicleTypeId = dto.VehicleTypeId,
                Year = dto.Year,
                Mileage = dto.Mileage,
                VIN = dto.VIN,
                FuelType = dto.FuelType,
                TransmissionType = dto.TransmissionType,
                BodyStyle = dto.BodyStyle,
                Condition = dto.Condition,
                EngineSize = dto.EngineSize,
                HorsePower = dto.HorsePower,
                Cylinders = dto.Cylinders,
                Color = dto.Color,
                InteriorColor = dto.InteriorColor,
                Doors = dto.Doors,
                Seats = dto.Seats,
                City = dto.City,
                Region = dto.Region,
                Description = dto.Description,
                Features = dto.Features,
                Price = dto.Price
            };

            await _unitOfWork.Vehicles.AddAsync(vehicle);

            // 2. Save images using ImageService
            if (dto.Images != null && dto.Images.Any())
            {
                var imageUrls = await _imageService.SaveVehicleImagesAsync(vehicle.Id, dto.Images);

                int order = 1;
                foreach (var url in imageUrls)
                {
                    var image = new VehicleImage
                    {
                        Id = Guid.NewGuid(),
                        VehicleId = vehicle.Id,
                        ImageUrl = url,
                        IsMain = order == 1,
                        DisplayOrder = order++
                    };

                    await _unitOfWork.VehicleImages.AddAsync(image);
                }
            }

            // 3. Create Advertisement
            var advertisement = new Advertisement
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                IsNegotiable = dto.IsNegotiable,
                Status = Domain.Enums.AdvertisementStatus.Active,
                PublishedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(60),
                ViewCount = 0,
                ContactCount = 0,
                FavoriteCount = 0
            };

            await _unitOfWork.Advertisements.AddAsync(advertisement);

            // 4. Save all
            await _unitOfWork.SaveChangesAsync();

            return advertisement.Id;
        }

        // ==================== UPDATE ====================

        public async Task<VehicleDto?> UpdateVehicleAsync(Guid id, CreateVehicleDto dto)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null) return null;

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

        // ==================== DELETE ====================

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null) return false;

            // Delete images from disk
            await _imageService.DeleteVehicleImagesAsync(id);

            _unitOfWork.Vehicles.Remove(vehicle);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // ==================== SEARCH ====================

        public async Task<IEnumerable<VehicleListDto>> SearchVehiclesAsync(
            Guid? makeId = null, Guid? modelId = null,
            int? yearFrom = null, int? yearTo = null,
            decimal? priceFrom = null, decimal? priceTo = null,
            int? mileageFrom = null, int? mileageTo = null,
            string? city = null)
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            foreach (var v in vehicles)
                await LoadVehicleRelatedDataAsync(v);

            var filtered = vehicles.AsEnumerable();

            if (makeId.HasValue) filtered = filtered.Where(v => v.MakeId == makeId.Value);
            if (modelId.HasValue) filtered = filtered.Where(v => v.ModelId == modelId.Value);
            if (yearFrom.HasValue) filtered = filtered.Where(v => v.Year >= yearFrom.Value);
            if (yearTo.HasValue) filtered = filtered.Where(v => v.Year <= yearTo.Value);
            if (priceFrom.HasValue) filtered = filtered.Where(v => v.Price >= priceFrom.Value);
            if (priceTo.HasValue) filtered = filtered.Where(v => v.Price <= priceTo.Value);
            if (mileageFrom.HasValue) filtered = filtered.Where(v => v.Mileage >= mileageFrom.Value);
            if (mileageTo.HasValue) filtered = filtered.Where(v => v.Mileage <= mileageTo.Value);
            if (!string.IsNullOrWhiteSpace(city)) filtered = filtered.Where(v => v.City.Contains(city));

            return filtered.Select(MapToListDto);
        }

        // ==================== FEATURED ====================

        public async Task<IEnumerable<VehicleListDto>> GetFeaturedVehiclesAsync(int count = 6)
        {
            var vehicles = (await _unitOfWork.Vehicles.GetAllAsync()).Take(count);

            foreach (var v in vehicles)
                await LoadVehicleRelatedDataAsync(v);

            return vehicles.Select(MapToListDto);
        }

        // ==================== LATEST ====================

        public async Task<IEnumerable<VehicleListDto>> GetLatestVehiclesAsync(int count = 10)
        {
            var vehicles = (await _unitOfWork.Vehicles.GetAllAsync())
                .OrderByDescending(v => v.CreatedAt)
                .Take(count);

            foreach (var v in vehicles)
                await LoadVehicleRelatedDataAsync(v);

            return vehicles.Select(MapToListDto);
        }

        // ==================== HELPERS ====================

        private static VehicleListDto MapToListDto(Vehicle v) => new()
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
        };

        private async Task LoadVehicleRelatedDataAsync(Vehicle vehicle)
        {
            if (vehicle.Make == null)
                vehicle.Make = await _unitOfWork.Makes.GetByIdAsync(vehicle.MakeId);

            if (vehicle.Model == null)
                vehicle.Model = await _unitOfWork.Models.GetByIdAsync(vehicle.ModelId);

            if (vehicle.Images == null || !vehicle.Images.Any())
            {
                var images = await _unitOfWork.VehicleImages.FindAsync(i => i.VehicleId == vehicle.Id);
                vehicle.Images = images.OrderBy(i => i.DisplayOrder).ToList();
            }
        }

        public async Task<IEnumerable<VehicleListDto>> GetUserAdvertisementsAsync(Guid userId)
        {
            var advertisements = await _unitOfWork.Advertisements.GetAllAsync();
            var userAds = advertisements.Where(a => a.UserId == userId && !a.IsDeleted).ToList();

            var result = new List<VehicleListDto>();

            foreach (var ad in userAds)
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(ad.VehicleId);
                if (vehicle != null)
                {
                    await LoadVehicleRelatedDataAsync(vehicle);
                    result.Add(MapToListDto(vehicle));
                }
            }

            return result.OrderByDescending(v => v.Year);
        }

        // ==================== GET USER STATS ====================

        public async Task<(int activeAds, int totalViews, int totalFavorites)> GetUserStatsAsync(Guid userId)
        {
            var advertisements = await _unitOfWork.Advertisements.GetAllAsync();
            var userAds = advertisements.Where(a => a.UserId == userId && !a.IsDeleted).ToList();

            var activeAds = userAds.Count(a => a.Status == Domain.Enums.AdvertisementStatus.Active);
            var totalViews = userAds.Sum(a => a.ViewCount);

            // Get favorites count
            var favorites = await _unitOfWork.Favorites.GetAllAsync();
            var adIds = userAds.Select(a => a.Id).ToList();
            var totalFavorites = favorites.Count(f => adIds.Contains(f.AdvertisementId));

            return (activeAds, totalViews, totalFavorites);
        }
    }
}