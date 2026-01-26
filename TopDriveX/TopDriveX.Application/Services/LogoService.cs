using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;

namespace TopDriveX.Application.Services
{
    public class LogoService : ILogoService
    {
        // Direct URLs to car logos
        private readonly Dictionary<string, string> _logoUrls = new()
        {
            { "BMW", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/bmw.svg" },
            { "Mercedes-Benz", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/mercedesbenz.svg" },
            { "Audi", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/audi.svg" },
            { "Toyota", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/toyota.svg" },
            { "Volkswagen", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/volkswagen.svg" },
            { "Honda", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/honda.svg" },
            { "Ford", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/ford.svg" },
            { "Renault", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/renault.svg" },
            { "Peugeot", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/peugeot.svg" },
            { "Skoda", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/skoda.svg" },
            { "Nissan", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/nissan.svg" },
            { "Mazda", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/mazda.svg" },
            { "Hyundai", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/hyundai.svg" },
            { "Kia", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/kia.svg" },
            { "Volvo", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/volvo.svg" },
            { "Porsche", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/porsche.svg" },
            { "Tesla", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/tesla.svg" },
            { "Lexus", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/lexus.svg" },
            { "Jaguar", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/jaguar.svg" },
            { "Land Rover", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/landrover.svg" },
            { "Ferrari", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/ferrari.svg" },
            { "Lamborghini", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/lamborghini.svg" },
            { "Maserati", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/maserati.svg" },
            { "Bentley", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/bentley.svg" },
            { "Rolls-Royce", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/rollsroyce.svg" },
            { "Subaru", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/subaru.svg" },
            { "Mitsubishi", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/mitsubishi.svg" },
            { "Suzuki", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/suzuki.svg" },
            { "Opel", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/opel.svg" },
            { "Fiat", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/fiat.svg" },
            { "Citroën", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/citroen.svg" },
            { "Alfa Romeo", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/alfaromeo.svg" },
            { "Seat", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/seat.svg" },
            { "Dacia", "https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/dacia.svg" }
        };

        public LogoService()
        {
        }

        /// <summary>
        /// Get logo URL from predefined dictionary
        /// </summary>
        public string GetMakeLogoUrl(string makeName)
        {
            if (string.IsNullOrWhiteSpace(makeName))
                return GetPlaceholderUrl(makeName);

            // Normalize make name
            makeName = makeName.Trim();

            // Try to find logo URL
            if (_logoUrls.TryGetValue(makeName, out var logoUrl))
            {
                return logoUrl;
            }

            // Fallback to placeholder
            return GetPlaceholderUrl(makeName);
        }

        /// <summary>
        /// Async version - same as sync for now
        /// </summary>
        public async Task<string> GetMakeLogoUrlAsync(string makeName)
        {
            return await Task.FromResult(GetMakeLogoUrl(makeName));
        }

        /// <summary>
        /// Generate inline SVG as data URL
        /// </summary>
        private string GetPlaceholderUrl(string makeName)
        {
            var letter = !string.IsNullOrEmpty(makeName) ? makeName[0].ToString().ToUpper() : "?";

            // Create inline SVG data URL (no external requests needed)
            var svg = $"data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='200' height='200'%3E%3Crect width='200' height='200' fill='%23dc2626'/%3E%3Ctext x='100' y='120' font-family='Arial,sans-serif' font-size='100' font-weight='bold' fill='white' text-anchor='middle'%3E{letter}%3C/text%3E%3C/svg%3E";

            return svg;
        }
    }
}