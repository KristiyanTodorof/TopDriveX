using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Application.Services
{
    public class MakeService : IMakeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MakeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MakeDto>> GetAllMakesAsync()
        {
            var makes = await _unitOfWork.Makes.GetAllAsync();

            return makes.Select(m => new MakeDto
            {
                Id = m.Id,
                Name = m.Name,
                LogoUrl = m.LogoUrl,
                Country = m.Country
            });
        }

        public async Task<MakeDto?> GetMakeByIdAsync(Guid id)
        {
            var make = await _unitOfWork.Makes.GetByIdAsync(id);

            if (make == null)
                return null;

            return new MakeDto
            {
                Id = make.Id,
                Name = make.Name,
                LogoUrl = make.LogoUrl,
                Country = make.Country
            };
        }

        public async Task<MakeDetailsDto?> GetMakeDetailsAsync(Guid id)
        {
            var make = await _unitOfWork.Makes.GetByIdAsync(id);
            if (make == null)
                return null;

            // Get all models for this make
            var models = await _unitOfWork.Models.FindAsync(m => m.MakeId == id);
            var modelsList = models.ToList();

            // Get all vehicles for this make
            var vehicles = await _unitOfWork.Vehicles.FindAsync(v => v.MakeId == id);
            var vehiclesList = vehicles.ToList();

            // Get all advertisements for vehicles of this make
            var vehicleIds = vehiclesList.Select(v => v.Id).ToList();
            var allAdvertisements = await _unitOfWork.Advertisements.GetAllAsync();
            var makeAdvertisements = allAdvertisements
                .Where(a => vehicleIds.Contains(a.VehicleId))
                .ToList();

            // Calculate statistics
            var activeAds = makeAdvertisements.Count(a =>
                a.Status == AdvertisementStatus.Active &&
                !a.IsDeleted);

            var averagePrice = vehiclesList.Any()
                ? vehiclesList.Average(v => v.Price)
                : 0;

            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            var newAds = makeAdvertisements.Count(a =>
                a.CreatedAt >= sevenDaysAgo &&
                !a.IsDeleted);

            // Map models with their vehicle counts
            var modelsDto = modelsList.Select(m => new ModelDto
            {
                Id = m.Id,
                MakeId = m.MakeId,
                Name = m.Name,
                MakeName = make.Name,
                YearFrom = m.YearFrom,
                YearTo = m.YearTo
            }).ToList();

            return new MakeDetailsDto
            {
                Id = make.Id,
                Name = make.Name,
                LogoUrl = make.LogoUrl,
                Country = make.Country,
                ActiveAdvertisementsCount = activeAds,
                AveragePrice = Math.Round(averagePrice, 0),
                NewAdvertisementsLast7Days = newAds,
                Models = modelsDto
            };
        }
    }
}
