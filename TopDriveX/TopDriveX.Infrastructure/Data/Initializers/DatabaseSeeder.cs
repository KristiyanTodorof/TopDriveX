using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Enums;
using TopDriveX.Domain.Models;

namespace TopDriveX.Infrastructure.Data.Initializers
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (await context.Vehicles.AnyAsync())
                {
                    logger.LogInformation("Database already contains vehicles. Skipping seeding.");
                    return;
                }

                logger.LogInformation("Starting data seeding...");

                // Seed Makes
                var makes = new List<Make>
                {
                    new Make { Name = "BMW", Country = "Germany" },
                    new Make { Name = "Mercedes-Benz", Country = "Germany" },
                    new Make { Name = "Audi", Country = "Germany" },
                    new Make { Name = "Toyota", Country = "Japan" },
                    new Make { Name = "Volkswagen", Country = "Germany" },
                    new Make { Name = "Honda", Country = "Japan" },
                    new Make { Name = "Ford", Country = "USA" },
                    new Make { Name = "Renault", Country = "France" },
                    new Make { Name = "Peugeot", Country = "France" },
                    new Make { Name = "Skoda", Country = "Czech Republic" }
                };

                await context.Makes.AddRangeAsync(makes);
                await context.SaveChangesAsync();

                // Seed Models
                var models = new List<Model>
                {
                    // BMW Models
                    new Model { MakeId = makes[0].Id, Name = "X5" },
                    new Model { MakeId = makes[0].Id, Name = "X3" },
                    new Model { MakeId = makes[0].Id, Name = "3 Series" },
                    new Model { MakeId = makes[0].Id, Name = "5 Series" },
                    
                    // Mercedes Models
                    new Model { MakeId = makes[1].Id, Name = "E-Class" },
                    new Model { MakeId = makes[1].Id, Name = "C-Class" },
                    new Model { MakeId = makes[1].Id, Name = "GLC" },
                    
                    // Audi Models
                    new Model { MakeId = makes[2].Id, Name = "A4" },
                    new Model { MakeId = makes[2].Id, Name = "A6" },
                    new Model { MakeId = makes[2].Id, Name = "Q5" },
                    
                    // Toyota Models
                    new Model { MakeId = makes[3].Id, Name = "Corolla" },
                    new Model { MakeId = makes[3].Id, Name = "Camry" },
                    new Model { MakeId = makes[3].Id, Name = "RAV4" },
                    
                    // VW Models
                    new Model { MakeId = makes[4].Id, Name = "Golf" },
                    new Model { MakeId = makes[4].Id, Name = "Passat" },
                    new Model { MakeId = makes[4].Id, Name = "Tiguan" },
                    
                    // Honda Models
                    new Model { MakeId = makes[5].Id, Name = "Civic" },
                    new Model { MakeId = makes[5].Id, Name = "Accord" },
                    new Model { MakeId = makes[5].Id, Name = "CR-V" }
                };

                await context.Models.AddRangeAsync(models);
                await context.SaveChangesAsync();

                // Seed Vehicles
                var random = new Random();
                var cities = new[] { "Sofai", "Plovdiv", "Varna", "Burgas", "Rousse", "Stara Zagora" };
                var colors = new[] { "Black", "White", "Silver", "Blue", "Black", "Gray" };
                var vehicles = new List<Vehicle>();

                foreach (var model in models)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var year = random.Next(2018, 2025);
                        var vehicle = new Vehicle
                        {
                            MakeId = model.MakeId,
                            ModelId = model.Id,
                            Year = year,
                            Mileage = random.Next(5000, 150000),
                            Price = random.Next(15000, 80000),
                            FuelType = (FuelType)random.Next(0, 5),
                            TransmissionType = (TransmissionType)random.Next(0, 2),
                            Condition = VehicleCondition.Used,
                            Color = colors[random.Next(colors.Length)],
                            City = cities[random.Next(cities.Length)],
                            Country = "Bulgaria",
                            Description = $"Perfectly preserved car from {year}.",
                            HorsePower = random.Next(100, 300),
                            Doors = random.Next(3, 6),
                            Seats = random.Next(4, 8)
                        };

                        vehicles.Add(vehicle);
                    }
                }

                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();

                // Seed Vehicle Images
                var vehicleImages = new List<VehicleImage>();
                foreach (var vehicle in vehicles)
                {
                    // Add 3 images per vehicle
                    for (int i = 0; i < 3; i++)
                    {
                        vehicleImages.Add(new VehicleImage
                        {
                            VehicleId = vehicle.Id,
                            ImageUrl = $"https://placehold.co/800x600/red/white?text=Car+{i + 1}",
                            IsMain = i == 0,
                            DisplayOrder = i
                        });
                    }
                }

                await context.VehicleImages.AddRangeAsync(vehicleImages);
                await context.SaveChangesAsync();

                logger.LogInformation($"Data seeding completed successfully. Created {vehicles.Count} vehicles.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }
    }
}
