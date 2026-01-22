using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Contracts
{
    public interface IModelService
    {
        Task<IEnumerable<ModelDto>> GetAllModelsAsync();
        Task<IEnumerable<ModelDto>> GetModelsByMakeIdAsync(Guid makeId);
        Task<ModelDto?> GetModelByIdAsync(Guid id);
    }
}
