using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Application.Dtos
{
    public class MakeDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Country { get; set; }

        // Statistics
        public int ActiveAdvertisementsCount { get; set; }
        public decimal AveragePrice { get; set; }
        public int NewAdvertisementsLast7Days { get; set; }

        // Models
        public List<ModelDto> Models { get; set; } = new();
    }
}
