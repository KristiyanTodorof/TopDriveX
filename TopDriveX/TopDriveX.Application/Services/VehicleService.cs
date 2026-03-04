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

        public async Task<EditAdvertisementDto?> GetAdvertisementForEditAsync(Guid advertisementId)
        {
            var advertisements = await _unitOfWork.Advertisements.GetAllAsync();
            var ad = advertisements.FirstOrDefault(a => a.Id == advertisementId);

            if (ad == null) return null;

            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(ad.VehicleId);
            if (vehicle == null) return null;

            await LoadVehicleRelatedDataAsync(vehicle);

            var model = new EditAdvertisementDto
            {
                AdvertisementId = ad.Id,
                VehicleId = vehicle.Id,
                OwnerId = ad.UserId,
                MakeId = vehicle.MakeId,
                ModelId = vehicle.ModelId,
                VehicleTypeId = vehicle.VehicleTypeId,
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                VIN = vehicle.VIN,
                FuelType = vehicle.FuelType,
                TransmissionType = vehicle.TransmissionType,
                BodyStyle = vehicle.BodyStyle ?? Domain.Enums.BodyStyle.Sedan,
                Condition = vehicle.Condition,
                HorsePower = vehicle.HorsePower,
                EngineSize = vehicle.EngineSize,
                Cylinders = vehicle.Cylinders,
                Color = vehicle.Color,
                InteriorColor = vehicle.InteriorColor,
                Doors = vehicle.Doors,
                Seats = vehicle.Seats,
                Title = ad.Title,
                Description = ad.Description ?? "",
                Price = ad.Price,
                IsNegotiable = ad.IsNegotiable,
                City = vehicle.City,
                Region = vehicle.Region,
                ExistingImages = vehicle.Images?
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageUrl)
                    .ToList() ?? new List<string>(),
                Features = vehicle.Features
            };

            return model;
        }

        public async Task<bool> UpdateAdvertisementAsync(Guid advertisementId, EditAdvertisementDto model, Guid currentUserId, bool isAdmin)
        {
            var advertisements = await _unitOfWork.Advertisements.GetAllAsync();
            var ad = advertisements.FirstOrDefault(a => a.Id == advertisementId);

            if (ad == null) return false;

            if (!isAdmin && ad.UserId != currentUserId) return false;

            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(ad.VehicleId);
            if (vehicle == null) return false;

            vehicle.MakeId = model.MakeId;
            vehicle.ModelId = model.ModelId;
            vehicle.VehicleTypeId = model.VehicleTypeId;
            vehicle.Year = model.Year;
            vehicle.Mileage = model.Mileage;
            vehicle.VIN = model.VIN;
            vehicle.FuelType = model.FuelType;
            vehicle.TransmissionType = model.TransmissionType;
            vehicle.BodyStyle = model.BodyStyle;
            vehicle.Condition = model.Condition;
            vehicle.HorsePower = model.HorsePower;
            vehicle.EngineSize = model.EngineSize;
            vehicle.Cylinders = model.Cylinders;
            vehicle.Color = model.Color;
            vehicle.InteriorColor = model.InteriorColor;
            vehicle.Doors = model.Doors;
            vehicle.Seats = model.Seats;
            vehicle.City = model.City;
            vehicle.Region = model.Region;
            vehicle.Description = model.Description;
            vehicle.Features = model.Features;
            vehicle.Price = model.Price;

            _unitOfWork.Vehicles.Update(vehicle);

            ad.Title = model.Title;
            ad.Description = model.Description;
            ad.Price = model.Price;
            ad.IsNegotiable = model.IsNegotiable;

            _unitOfWork.Advertisements.Update(ad);

            if (!string.IsNullOrEmpty(model.ImagesToDeleteJson) && model.ImagesToDeleteJson != "[]")
            {
                var imagesToDelete = System.Text.Json.JsonSerializer.Deserialize<List<string>>(model.ImagesToDeleteJson);
                if (imagesToDelete != null && imagesToDelete.Any())
                {
                    var existingImages = await _unitOfWork.VehicleImages.FindAsync(i => i.VehicleId == vehicle.Id);
                    foreach (var imageUrl in imagesToDelete)
                    {
                        var imageToDelete = existingImages.FirstOrDefault(i => i.ImageUrl == imageUrl);
                        if (imageToDelete != null)
                        {
                            _unitOfWork.VehicleImages.Remove(imageToDelete);
                        }
                    }
                }
            }

            if (model.NewImages != null && model.NewImages.Any())
            {
                var newImageUrls = await _imageService.SaveVehicleImagesAsync(vehicle.Id, model.NewImages);
                var maxOrder = (await _unitOfWork.VehicleImages.FindAsync(i => i.VehicleId == vehicle.Id))
                    .OrderByDescending(i => i.DisplayOrder)
                    .FirstOrDefault()?.DisplayOrder ?? 0;

                int order = maxOrder + 1;
                foreach (var url in newImageUrls)
                {
                    var image = new VehicleImage
                    {
                        Id = Guid.NewGuid(),
                        VehicleId = vehicle.Id,
                        ImageUrl = url,
                        IsMain = false,
                        DisplayOrder = order++
                    };
                    await _unitOfWork.VehicleImages.AddAsync(image);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAdvertisementAsync(Guid advertisementId, Guid currentUserId, bool isAdmin)
        {
            var advertisements = await _unitOfWork.Advertisements.GetAllAsync();
            var ad = advertisements.FirstOrDefault(a => a.Id == advertisementId);

            if (ad == null) return false;
            if (!isAdmin && ad.UserId != currentUserId) return false;

            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(ad.VehicleId);
            if (vehicle != null)
            {
                await _imageService.DeleteVehicleImagesAsync(vehicle.Id);
                var images = await _unitOfWork.VehicleImages.FindAsync(i => i.VehicleId == vehicle.Id);
                foreach (var img in images) _unitOfWork.VehicleImages.Remove(img);
                _unitOfWork.Vehicles.Remove(vehicle);
            }

            _unitOfWork.Advertisements.Remove(ad);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}