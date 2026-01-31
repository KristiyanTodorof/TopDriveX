using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Dtos;

namespace TopDriveX.Application.Contracts
{
    public interface IMakeService
    {
        Task<IEnumerable<MakeDto>> GetAllMakesAsync();
        Task<MakeDto?> GetMakeByIdAsync(Guid id);
        Task<MakeDetailsDto?> GetMakeDetailsAsync(Guid id);
    }
}
