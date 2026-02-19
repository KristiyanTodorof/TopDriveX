using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Contracts
{
    public interface IImageService
    {
        Task<List<string>> SaveVehicleImagesAsync(Guid vehicleId, List<IFormFile> images);

        Task DeleteVehicleImagesAsync(Guid vehicleId);
    }
}
