using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Contracts
{
    public interface ILogoService
    {
        Task<string> GetMakeLogoUrlAsync(string makeName);
        string GetMakeLogoUrl(string makeName);
    }
}
