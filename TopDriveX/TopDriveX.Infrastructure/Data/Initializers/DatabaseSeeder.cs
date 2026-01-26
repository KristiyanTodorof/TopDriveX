using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopDriveX.Domain.Enums;
using TopDriveX.Domain.Models;
using TopDriveX.Infrastructure.Data;

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

                // ==================== SEED MAKES ====================
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
                    new Make { Name = "Skoda", Country = "Czech Republic" },
                    new Make { Name = "Mazda", Country = "Japan" },
                    new Make { Name = "Nissan", Country = "Japan" },
                    new Make { Name = "Hyundai", Country = "South Korea" },
                    new Make { Name = "Kia", Country = "South Korea" },
                    new Make { Name = "Volvo", Country = "Sweden" },
                    new Make { Name = "Opel", Country = "Germany" },
                    new Make { Name = "Citroën", Country = "France" },
                    new Make { Name = "Fiat", Country = "Italy" },
                    new Make { Name = "Seat", Country = "Spain" },
                    new Make { Name = "Dacia", Country = "Romania" }
                };

                await context.Makes.AddRangeAsync(makes);
                await context.SaveChangesAsync();

                // ==================== SEED MODELS ====================
                var models = new List<Model>
                {
                    // BMW Models
                    new Model { MakeId = makes[0].Id, Name = "X1", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[0].Id, Name = "X3", YearFrom = 2017, YearTo = null },
                    new Model { MakeId = makes[0].Id, Name = "X5", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[0].Id, Name = "3 Series", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[0].Id, Name = "5 Series", YearFrom = 2010, YearTo = null },
                    new Model { MakeId = makes[0].Id, Name = "7 Series", YearFrom = 2015, YearTo = null },
                    
                    // Mercedes-Benz Models
                    new Model { MakeId = makes[1].Id, Name = "A-Class", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "C-Class", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "E-Class", YearFrom = 2016, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "S-Class", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "GLA", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "GLC", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "GLE", YearFrom = 2015, YearTo = null },
                    
                    // Audi Models
                    new Model { MakeId = makes[2].Id, Name = "A3", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "A4", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "A6", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "Q3", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "Q5", YearFrom = 2016, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "Q7", YearFrom = 2015, YearTo = null },
                    
                    // Toyota Models
                    new Model { MakeId = makes[3].Id, Name = "Corolla", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[3].Id, Name = "Camry", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[3].Id, Name = "RAV4", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[3].Id, Name = "Yaris", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[3].Id, Name = "Avensis", YearFrom = 2009, YearTo = 2018 },
                    new Model { MakeId = makes[3].Id, Name = "Land Cruiser", YearFrom = 2007, YearTo = null },
                    
                    // Volkswagen Models
                    new Model { MakeId = makes[4].Id, Name = "Golf", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[4].Id, Name = "Passat", YearFrom = 2010, YearTo = null },
                    new Model { MakeId = makes[4].Id, Name = "Tiguan", YearFrom = 2016, YearTo = null },
                    new Model { MakeId = makes[4].Id, Name = "Polo", YearFrom = 2009, YearTo = null },
                    new Model { MakeId = makes[4].Id, Name = "Touareg", YearFrom = 2010, YearTo = null },
                    
                    // Honda Models
                    new Model { MakeId = makes[5].Id, Name = "Civic", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[5].Id, Name = "Accord", YearFrom = 2008, YearTo = null },
                    new Model { MakeId = makes[5].Id, Name = "CR-V", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[5].Id, Name = "Jazz", YearFrom = 2008, YearTo = null },
                    
                    // Ford Models
                    new Model { MakeId = makes[6].Id, Name = "Focus", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[6].Id, Name = "Fiesta", YearFrom = 2008, YearTo = null },
                    new Model { MakeId = makes[6].Id, Name = "Mondeo", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[6].Id, Name = "Kuga", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[6].Id, Name = "Mustang", YearFrom = 2015, YearTo = null },
                    
                    // Renault Models
                    new Model { MakeId = makes[7].Id, Name = "Clio", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[7].Id, Name = "Megane", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[7].Id, Name = "Captur", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[7].Id, Name = "Kadjar", YearFrom = 2015, YearTo = null },
                    
                    // Peugeot Models
                    new Model { MakeId = makes[8].Id, Name = "208", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[8].Id, Name = "308", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[8].Id, Name = "3008", YearFrom = 2016, YearTo = null },
                    new Model { MakeId = makes[8].Id, Name = "5008", YearFrom = 2017, YearTo = null },
                    
                    // Skoda Models
                    new Model { MakeId = makes[9].Id, Name = "Octavia", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[9].Id, Name = "Superb", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[9].Id, Name = "Fabia", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[9].Id, Name = "Kodiaq", YearFrom = 2016, YearTo = null },
                    
                    // Mazda Models
                    new Model { MakeId = makes[10].Id, Name = "3", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[10].Id, Name = "6", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[10].Id, Name = "CX-5", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[10].Id, Name = "MX-5", YearFrom = 2015, YearTo = null },
                    
                    // Nissan Models
                    new Model { MakeId = makes[11].Id, Name = "Qashqai", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[11].Id, Name = "Juke", YearFrom = 2010, YearTo = null },
                    new Model { MakeId = makes[11].Id, Name = "X-Trail", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[11].Id, Name = "Micra", YearFrom = 2010, YearTo = null },
                    
                    // Hyundai Models
                    new Model { MakeId = makes[12].Id, Name = "i20", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[12].Id, Name = "i30", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[12].Id, Name = "Tucson", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[12].Id, Name = "Santa Fe", YearFrom = 2012, YearTo = null },
                    
                    // Kia Models
                    new Model { MakeId = makes[13].Id, Name = "Sportage", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[13].Id, Name = "Ceed", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[13].Id, Name = "Rio", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[13].Id, Name = "Sorento", YearFrom = 2015, YearTo = null }
                };

                await context.Models.AddRangeAsync(models);
                await context.SaveChangesAsync();

                // ==================== SEED VEHICLES ====================
                var random = new Random(42); // Fixed seed for consistency
                var cities = new[] { "София", "Пловдив", "Варна", "Бургас", "Русе", "Стара Загора", "Велико Търново", "Плевен" };
                var colors = new[] { "Черен", "Бял", "Сребрист", "Син", "Червен", "Сив", "Зелен", "Бежов" };
                var vehicles = new List<Vehicle>();

                // Create 3-5 vehicles per model
                foreach (var model in models)
                {
                    int vehicleCount = random.Next(3, 6);

                    for (int i = 0; i < vehicleCount; i++)
                    {
                        int year = random.Next(Math.Max(2015, model.YearFrom ?? 2015), 2025);
                        int mileage = (2025 - year) * random.Next(8000, 25000);

                        var vehicle = new Vehicle
                        {
                            MakeId = model.MakeId,
                            ModelId = model.Id,
                            Year = year,
                            Mileage = mileage,
                            Price = CalculatePrice(model.Make.Name, year, mileage, random),
                            FuelType = GetRandomFuelType(random),
                            TransmissionType = random.Next(0, 100) < 70 ? TransmissionType.Automatic : TransmissionType.Manual,
                            Condition = year >= 2023 ? VehicleCondition.New : VehicleCondition.Used,
                            BodyStyle = GetBodyStyle(model.Name),
                            Color = colors[random.Next(colors.Length)],
                            InteriorColor = colors[random.Next(colors.Length)],
                            City = cities[random.Next(cities.Length)],
                            Country = "Bulgaria",
                            Description = GenerateDescription(model.Make.Name, model.Name, year),
                            HorsePower = random.Next(90, 350),
                            EngineSize = Math.Round((decimal)(random.Next(12, 40) / 10.0), 1),
                            Cylinders = random.Next(3, 9),
                            Doors = GetDoors(model.Name),
                            Seats = GetSeats(model.Name)
                        };

                        vehicles.Add(vehicle);
                    }
                }

                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();

                // ==================== SEED VEHICLE IMAGES ====================
                var vehicleImages = new List<VehicleImage>();
                foreach (var vehicle in vehicles)
                {
                    int imageCount = random.Next(3, 6);
                    for (int i = 0; i < imageCount; i++)
                    {
                        vehicleImages.Add(new VehicleImage
                        {
                            VehicleId = vehicle.Id,
                            ImageUrl = $"https://placehold.co/800x600/dc2626/ffffff?text={vehicle.Make.Name}+{vehicle.Model.Name}",
                            IsMain = i == 0,
                            DisplayOrder = i,
                            Caption = i == 0 ? "Главна снимка" : $"Снимка {i + 1}"
                        });
                    }
                }

                await context.VehicleImages.AddRangeAsync(vehicleImages);
                await context.SaveChangesAsync();

                logger.LogInformation($"Data seeding completed successfully.");
                logger.LogInformation($"Created {makes.Count} makes, {models.Count} models, {vehicles.Count} vehicles.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        // Helper methods
        private static decimal CalculatePrice(string makeName, int year, int mileage, Random random)
        {
            decimal basePrice = makeName switch
            {
                "BMW" => 45000,
                "Mercedes-Benz" => 48000,
                "Audi" => 42000,
                "Toyota" => 25000,
                "Volkswagen" => 28000,
                "Honda" => 24000,
                "Ford" => 22000,
                "Renault" => 18000,
                "Peugeot" => 19000,
                "Skoda" => 23000,
                "Mazda" => 26000,
                "Nissan" => 24000,
                "Hyundai" => 22000,
                "Kia" => 21000,
                "Volvo" => 38000,
                _ => 20000
            };

            // Adjust for year
            decimal yearFactor = (year - 2015) / 10.0m;
            basePrice = basePrice * (1 + yearFactor);

            // Adjust for mileage
            decimal mileageFactor = mileage / 200000.0m;
            basePrice = basePrice * (1 - mileageFactor);

            // Add randomness
            basePrice = basePrice * (1 + (decimal)(random.NextDouble() * 0.2 - 0.1));

            return Math.Round(basePrice, 0);
        }

        private static FuelType GetRandomFuelType(Random random)
        {
            int choice = random.Next(0, 100);
            return choice switch
            {
                < 40 => FuelType.Diesel,
                < 70 => FuelType.Petrol,
                < 85 => FuelType.Hybrid,
                < 95 => FuelType.Electric,
                _ => FuelType.PlugInHybrid
            };
        }

        private static BodyStyle GetBodyStyle(string modelName)
        {
            if (modelName.Contains("X") || modelName.Contains("Q") || modelName.Contains("Tiguan") ||
                modelName.Contains("Tucson") || modelName.Contains("RAV") || modelName.Contains("CR-V") ||
                modelName.Contains("Captur") || modelName.Contains("3008") || modelName.Contains("Qashqai") ||
                modelName.Contains("Kodiaq") || modelName.Contains("CX") || modelName.Contains("Sportage"))
                return BodyStyle.SUV;

            if (modelName.Contains("Coupe") || modelName.Contains("Mustang"))
                return BodyStyle.Coupe;

            if (modelName.Contains("Kombi") || modelName.Contains("Estate"))
                return BodyStyle.Wagon;

            return BodyStyle.Sedan;
        }

        private static int GetDoors(string modelName)
        {
            return modelName.Contains("Coupe") || modelName.Contains("MX-5") ? 2 :
                   modelName.Contains("X") || modelName.Contains("Q") || modelName.Contains("SUV") ? 5 : 4;
        }

        private static int GetSeats(string modelName)
        {
            return modelName.Contains("5008") || modelName.Contains("Sorento") ||
                   modelName.Contains("Land Cruiser") ? 7 : 5;
        }

        private static string GenerateDescription(string make, string model, int year)
        {
            var descriptions = new[]
            {
                $"Отлично запазен {make} {model} от {year} година. Перфектно състояние, редовна поддръжка.",
                $"Много добро състояние. {make} {model} {year}г. с пълна история на обслужване.",
                $"Красив {make} {model} в отлично техническо състояние. Година {year}.",
                $"Еднособственик! {make} {model} {year}г. Редовна поддръжка, гаражирана.",
                $"Топ състояние! {make} {model} от {year}г. Внос от Германия.",
                $"Перфектен {make} {model}! Година {year}. Пълна документация."
            };

            return descriptions[new Random(make.GetHashCode() + year).Next(descriptions.Length)];
        }
    }
}