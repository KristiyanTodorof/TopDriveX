using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Services
{
    public class MakeService : IMakeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MakeService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
    }
}
