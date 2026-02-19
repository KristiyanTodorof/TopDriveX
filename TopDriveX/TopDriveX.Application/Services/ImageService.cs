using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;

namespace TopDriveX.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly string _webRootPath;

        public ImageService(IConfiguration configuration)
        {
            // Read from appsettings or use default
            _webRootPath = configuration["FileUpload:WebRootPath"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public async Task<List<string>> SaveVehicleImagesAsync(Guid vehicleId, List<IFormFile> images)
        {
            var imageUrls = new List<string>();

            if (images == null || !images.Any())
                return imageUrls;

            // Construct upload path
            var uploadPath = Path.Combine(_webRootPath, "uploads", "vehicles", vehicleId.ToString());
            Directory.CreateDirectory(uploadPath);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            const long maxFileSize = 10 * 1024 * 1024; // 10MB

            foreach (var file in images.Take(15))
            {
                if (file.Length == 0) continue;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension)) continue;

                if (file.Length > maxFileSize) continue;

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                imageUrls.Add($"/uploads/vehicles/{vehicleId}/{fileName}");
            }

            return imageUrls;
        }

        public Task DeleteVehicleImagesAsync(Guid vehicleId)
        {
            var uploadPath = Path.Combine(_webRootPath, "uploads", "vehicles", vehicleId.ToString());

            if (Directory.Exists(uploadPath))
            {
                Directory.Delete(uploadPath, recursive: true);
            }

            return Task.CompletedTask;
        }
    }
}
