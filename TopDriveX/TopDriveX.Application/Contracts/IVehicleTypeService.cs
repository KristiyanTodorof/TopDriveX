using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Contracts
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync();
        Task<VehicleTypeDto?> GetVehicleTypeByIdAsync(Guid id);
    }
}
