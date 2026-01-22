using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync()
        {
            var vehicleTypes = await _unitOfWork.VehicleTypes.GetAllAsync();

            return vehicleTypes.Select(vt => new VehicleTypeDto
            {
                Id = vt.Id,
                Name = vt.Name,
                Description = vt.Description
            });
        }

        public async Task<VehicleTypeDto?> GetVehicleTypeByIdAsync(Guid id)
        {
            var vehicleType = await _unitOfWork.VehicleTypes.GetByIdAsync(id);

            if (vehicleType == null)
                return null;

            return new VehicleTypeDto
            {
                Id = vehicleType.Id,
                Name = vehicleType.Name,
                Description = vehicleType.Description
            };
        }
    }
}
