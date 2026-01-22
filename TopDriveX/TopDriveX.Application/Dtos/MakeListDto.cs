using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class MakeListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ModelsCount { get; set; }
    }
}
