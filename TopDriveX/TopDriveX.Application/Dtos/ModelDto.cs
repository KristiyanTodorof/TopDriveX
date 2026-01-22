using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class ModelDto
    {
        public Guid Id { get; set; }
        public Guid MakeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MakeName { get; set; } = string.Empty;
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
    }
}
