using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Contracts
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleListDto>> GetAllVehiclesAsync();
        Task<VehicleDto?> GetVehicleByIdAsync(Guid id);
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto);
        Task<VehicleDto?> UpdateVehicleAsync(Guid id, CreateVehicleDto dto);
        Task<bool> DeleteVehicleAsync(Guid id);

        // Search & Filter
        Task<IEnumerable<VehicleListDto>> SearchVehiclesAsync(
            Guid? makeId = null,
            Guid? modelId = null,
            int? yearFrom = null,
            int? yearTo = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            int? mileageFrom = null,
            int? mileageTo = null,
            string? city = null);

        Task<IEnumerable<VehicleListDto>> GetFeaturedVehiclesAsync(int count = 6);
        Task<IEnumerable<VehicleListDto>> GetLatestVehiclesAsync(int count = 10);
    }
}
