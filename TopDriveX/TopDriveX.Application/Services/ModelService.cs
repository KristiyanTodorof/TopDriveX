using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Services
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModelService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ModelDto>> GetAllModelsAsync()
        {
            var models = await _unitOfWork.Models.GetAllAsync();

            return models.Select(m => new ModelDto
            {
                Id = m.Id,
                MakeId = m.MakeId,
                Name = m.Name,
                MakeName = m.Make?.Name ?? "",
                YearFrom = m.YearFrom,
                YearTo = m.YearTo
            });
        }

        public async Task<IEnumerable<ModelDto>> GetModelsByMakeIdAsync(Guid makeId)
        {
            var models = await _unitOfWork.Models.FindAsync(m => m.MakeId == makeId);

            return models.Select(m => new ModelDto
            {
                Id = m.Id,
                MakeId = m.MakeId,
                Name = m.Name,
                MakeName = m.Make?.Name ?? "",
                YearFrom = m.YearFrom,
                YearTo = m.YearTo
            });
        }

        public async Task<ModelDto?> GetModelByIdAsync(Guid id)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);

            if (model == null)
                return null;

            return new ModelDto
            {
                Id = model.Id,
                MakeId = model.MakeId,
                Name = model.Name,
                MakeName = model.Make?.Name ?? "",
                YearFrom = model.YearFrom,
                YearTo = model.YearTo
            };
        }
    }
}
