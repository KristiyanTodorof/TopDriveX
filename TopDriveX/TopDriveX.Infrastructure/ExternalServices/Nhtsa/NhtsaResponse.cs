using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Infrastructure.ExternalServices.Nhtsa
{
    public class NhtsaResponse<T>
    {
        public int Count { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<T> Results { get; set; } = new();
    }
}
