using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                    
                    // Mercedes-Benz Models
                    new Model { MakeId = makes[1].Id, Name = "A-Class", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "C-Class", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "E-Class", YearFrom = 2016, YearTo = null },
                    new Model { MakeId = makes[1].Id, Name = "GLE", YearFrom = 2015, YearTo = null },
                    
                    // Audi Models
                    new Model { MakeId = makes[2].Id, Name = "A3", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "A4", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[2].Id, Name = "Q5", YearFrom = 2016, YearTo = null },
                    
                    // Toyota Models
                    new Model { MakeId = makes[3].Id, Name = "Corolla", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[3].Id, Name = "RAV4", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[3].Id, Name = "Yaris", YearFrom = 2011, YearTo = null },
                    
                    // Volkswagen Models
                    new Model { MakeId = makes[4].Id, Name = "Golf", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[4].Id, Name = "Passat", YearFrom = 2010, YearTo = null },
                    new Model { MakeId = makes[4].Id, Name = "Tiguan", YearFrom = 2016, YearTo = null },
                    
                    // Honda Models
                    new Model { MakeId = makes[5].Id, Name = "Civic", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[5].Id, Name = "Accord", YearFrom = 2008, YearTo = null },
                    new Model { MakeId = makes[5].Id, Name = "CR-V", YearFrom = 2012, YearTo = null },
                    
                    // Ford Models
                    new Model { MakeId = makes[6].Id, Name = "Focus", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[6].Id, Name = "Fiesta", YearFrom = 2008, YearTo = null },
                    new Model { MakeId = makes[6].Id, Name = "Mondeo", YearFrom = 2014, YearTo = null },
                    
                    // Renault Models
                    new Model { MakeId = makes[7].Id, Name = "Clio", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[7].Id, Name = "Megane", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[7].Id, Name = "Captur", YearFrom = 2013, YearTo = null },
                    
                    // Peugeot Models
                    new Model { MakeId = makes[8].Id, Name = "208", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[8].Id, Name = "308", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[8].Id, Name = "3008", YearFrom = 2016, YearTo = null },
                    
                    // Skoda Models
                    new Model { MakeId = makes[9].Id, Name = "Octavia", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[9].Id, Name = "Superb", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[9].Id, Name = "Fabia", YearFrom = 2014, YearTo = null },
                    
                    // Mazda Models
                    new Model { MakeId = makes[10].Id, Name = "3", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[10].Id, Name = "6", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[10].Id, Name = "CX-5", YearFrom = 2012, YearTo = null },
                    
                    // Nissan Models
                    new Model { MakeId = makes[11].Id, Name = "Qashqai", YearFrom = 2013, YearTo = null },
                    new Model { MakeId = makes[11].Id, Name = "Juke", YearFrom = 2010, YearTo = null },
                    new Model { MakeId = makes[11].Id, Name = "X-Trail", YearFrom = 2014, YearTo = null },
                    
                    // Hyundai Models
                    new Model { MakeId = makes[12].Id, Name = "i20", YearFrom = 2014, YearTo = null },
                    new Model { MakeId = makes[12].Id, Name = "i30", YearFrom = 2012, YearTo = null },
                    new Model { MakeId = makes[12].Id, Name = "Tucson", YearFrom = 2015, YearTo = null },
                    
                    // Kia Models
                    new Model { MakeId = makes[13].Id, Name = "Rio", YearFrom = 2011, YearTo = null },
                    new Model { MakeId = makes[13].Id, Name = "Sportage", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[13].Id, Name = "Ceed", YearFrom = 2012, YearTo = null },
                    
                    // Volvo Models
                    new Model { MakeId = makes[14].Id, Name = "XC60", YearFrom = 2017, YearTo = null },
                    new Model { MakeId = makes[14].Id, Name = "XC90", YearFrom = 2015, YearTo = null },
                    new Model { MakeId = makes[14].Id, Name = "V40", YearFrom = 2012, YearTo = null },
                };

                await context.Models.AddRangeAsync(models);
                await context.SaveChangesAsync();

                logger.LogInformation("Makes and models seeded successfully");

                // ==================== SEED VEHICLES ====================
                var vehicles = new List<Vehicle>();
                var random = new Random(42);
                var cities = new[] { "София", "Пловдив", "Варна", "Бургас", "Русе", "Стара Загора", "Плевен" };

                // Add 50 vehicles with varied data
                for (int i = 0; i < 50; i++)
                {
                    var make = makes[random.Next(makes.Count)];
                    var makeModels = models.Where(m => m.MakeId == make.Id).ToList();
                    if (makeModels.Count == 0) continue;

                    var model = makeModels[random.Next(makeModels.Count)];
                    var year = random.Next(2015, 2024);
                    var mileage = random.Next(10000, 200000);

                    vehicles.Add(new Vehicle
                    {
                        MakeId = make.Id,
                        ModelId = model.Id,
                        Year = year,
                        Mileage = mileage,
                        Price = CalculatePrice(make.Name, year, mileage, random),
                        FuelType = GetRandomFuelType(random),
                        TransmissionType = random.Next(2) == 0 ? TransmissionType.Manual : TransmissionType.Automatic,
                        BodyStyle = GetBodyStyle(model.Name),
                        Condition = VehicleCondition.Used,
                        Color = GetRandomColor(random),
                        City = cities[random.Next(cities.Length)],
                        Description = GenerateDescription(make.Name, model.Name, year),
                        Doors = GetDoors(model.Name),
                        Seats = GetSeats(model.Name),
                        EngineSize = GetEngineSize(random),
                        HorsePower = random.Next(90, 350)
                    });
                }

                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();

                logger.LogInformation($"Vehicles seeded: {vehicles.Count}");

                // ==================== SEED ADVERTISEMENTS ====================
                var advertisements = new List<Advertisement>();
                var users = new List<User>();

                // Create some demo users first
                for (int i = 0; i < 5; i++)
                {
                    users.Add(new User
                    {
                        UserName = $"user{i + 1}",
                        Email = $"user{i + 1}@topdrivex.com",
                        PasswordHash = "dummy_hash_" + i, // In production, use proper hashing
                        FirstName = $"User",
                        LastName = $"{i + 1}",
                        PhoneNumber = $"08{random.Next(10000000, 99999999)}",
                        UserType = i == 0 ? UserType.Admin : (i < 3 ? UserType.Dealer : UserType.Private),
                        City = cities[random.Next(cities.Length)],
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        IsActive = true
                    });
                }

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();

                logger.LogInformation($"Users seeded: {users.Count}");

                // Create advertisements for vehicles (70% of vehicles get an ad)
                foreach (var vehicle in vehicles)
                {
                    if (random.Next(100) < 70)
                    {
                        var make = makes.First(m => m.Id == vehicle.MakeId);
                        var model = models.First(m => m.Id == vehicle.ModelId);
                        var user = users[random.Next(users.Count)];

                        // Determine status
                        var statusRoll = random.Next(100);
                        var status = statusRoll switch
                        {
                            < 80 => AdvertisementStatus.Active,
                            < 90 => AdvertisementStatus.Pending,
                            < 95 => AdvertisementStatus.Sold,
                            _ => AdvertisementStatus.Expired
                        };

                        advertisements.Add(new Advertisement
                        {
                            VehicleId = vehicle.Id,
                            UserId = user.Id,
                            Title = $"{make.Name} {model.Name} {vehicle.Year}г. - {vehicle.City}",
                            Description = vehicle.Description ?? "Отличен автомобил в много добро състояние.",
                            Price = vehicle.Price,
                            IsNegotiable = random.Next(2) == 0,
                            Status = status,
                            IsFeatured = random.Next(100) < 20, // 20% са featured
                            FeaturedUntil = random.Next(100) < 20 ? DateTime.UtcNow.AddDays(random.Next(7, 30)) : null,
                            PublishedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                            ExpiresAt = DateTime.UtcNow.AddDays(random.Next(30, 90)),
                            ViewCount = random.Next(0, 500),
                            ContactCount = random.Next(0, 50),
                            FavoriteCount = random.Next(0, 20)
                        });
                    }
                }

                await context.Advertisements.AddRangeAsync(advertisements);
                await context.SaveChangesAsync();

                logger.LogInformation($"Advertisements seeded: {advertisements.Count}");

                // ==================== NO IMAGES - REMOVED ====================
                // VehicleImages table will remain empty
                // Frontend should handle missing images gracefully

                logger.LogInformation($"Data seeding completed successfully.");
                logger.LogInformation($"Created {makes.Count} makes, {models.Count} models, {vehicles.Count} vehicles, {users.Count} users, {advertisements.Count} advertisements.");
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

            decimal yearFactor = (year - 2015) / 10.0m;
            basePrice = basePrice * (1 + yearFactor);

            decimal mileageFactor = mileage / 200000.0m;
            basePrice = basePrice * (1 - mileageFactor);

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
                modelName.Contains("Captur") || modelName.Contains("Qashqai") || modelName.Contains("Kodiaq") ||
                modelName.Contains("CX") || modelName.Contains("Sportage") || modelName.Contains("GLE") ||
                modelName.Contains("XC"))
                return BodyStyle.SUV;

            if (modelName.Contains("Coupe") || modelName.Contains("Mustang"))
                return BodyStyle.Coupe;

            if (modelName.Contains("Kombi") || modelName.Contains("Estate"))
                return BodyStyle.Wagon;

            return BodyStyle.Sedan;
        }

        private static int GetDoors(string modelName)
        {
            return modelName.Contains("Coupe") ? 2 :
                   modelName.Contains("X") || modelName.Contains("Q") || modelName.Contains("SUV") ? 5 : 4;
        }

        private static int GetSeats(string modelName)
        {
            return modelName.Contains("Land Cruiser") || modelName.Contains("XC90") ? 7 : 5;
        }

        private static decimal GetEngineSize(Random random)
        {
            var sizes = new[] { 1.2m, 1.4m, 1.6m, 1.8m, 2.0m, 2.2m, 2.5m, 3.0m };
            return sizes[random.Next(sizes.Length)];
        }

        private static string GetRandomColor(Random random)
        {
            var colors = new[] { "Черен", "Бял", "Сребрист", "Син", "Червен", "Сив", "Зелен", "Тъмносин" };
            return colors[random.Next(colors.Length)];
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
                $"Перфектен {make} {model}! Година {year}. Пълна документация.",
                $"{make} {model} {year}г. - надеждна кола за дълги пътувания.",
                $"Икономична и комфортна кола. {make} {model} {year}г."
            };

            return descriptions[new Random(make.GetHashCode() + year).Next(descriptions.Length)];
        }
    }
}