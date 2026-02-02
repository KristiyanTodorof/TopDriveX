using Microsoft.AspNetCore.Identity;
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
    public static class IdentitySeeder
    {
        /// <summary>
        /// Main seeding method - creates roles and admin user
        /// </summary>
        public static async Task SeedAsync(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ILogger logger)
        {
            try
            {
                logger.LogInformation("Starting Identity seeding...");

                // Step 1: Create Roles
                await SeedRolesAsync(roleManager, logger);

                // Step 2: Create Admin User
                await SeedAdminUserAsync(userManager, logger);

                // Step 3: Create Demo Users (optional)
                await SeedDemoUsersAsync(userManager, logger);

                logger.LogInformation("Identity seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding identity data");
                throw;
            }
        }

        // ==================== SEED ROLES ====================

        /// <summary>
        /// Creates three roles: Admin, Dealer, Private
        /// </summary>
        private static async Task SeedRolesAsync(
            RoleManager<IdentityRole<Guid>> roleManager,
            ILogger logger)
        {
            logger.LogInformation("Seeding roles...");

            // Define all roles
            string[] roleNames = { "Admin", "Dealer", "Private" };

            foreach (var roleName in roleNames)
            {
                // Check if role already exists
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };

                    var result = await roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        logger.LogInformation($"✅ Role '{roleName}' created successfully");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"❌ Failed to create role '{roleName}': {errors}");
                    }
                }
                else
                {
                    logger.LogInformation($"ℹ️ Role '{roleName}' already exists, skipping");
                }
            }

            logger.LogInformation($"Roles seeded: {roleNames.Length}");
        }

        // ==================== SEED ADMIN USER ====================

        /// <summary>
        /// Creates the default admin user
        /// Email: admin@topdrivex.bg
        /// Password: Admin123!
        /// </summary>
        private static async Task SeedAdminUserAsync(
            UserManager<User> userManager,
            ILogger logger)
        {
            logger.LogInformation("Seeding admin user...");

            const string adminEmail = "admin@topdrivex.bg";
            const string adminUsername = "admin";
            const string adminPassword = "Admin123!";

            // Check if admin already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Create new admin user
                adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = adminUsername,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "TopDriveX",
                    PhoneNumber = "+359888916060",
                    UserType = UserType.Admin,
                    City = "Тутракан",
                    Country = "Bulgaria",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Create user with password (UserManager handles hashing)
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Assign Admin role
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    logger.LogInformation("✅ Admin user created successfully");
                    logger.LogInformation("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    logger.LogInformation("📧 Email:    {Email}", adminEmail);
                    logger.LogInformation("👤 Username: {Username}", adminUsername);
                    logger.LogInformation("🔑 Password: {Password}", adminPassword);
                    logger.LogInformation("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError($"❌ Failed to create admin user: {errors}");
                }
            }
            else
            {
                logger.LogInformation("ℹ️ Admin user already exists, skipping");
            }
        }

        // ==================== SEED DEMO USERS ====================

        /// <summary>
        /// Creates demo users for testing
        /// 2 Dealers, 3 Private users
        /// </summary>
        private static async Task SeedDemoUsersAsync(
            UserManager<User> userManager,
            ILogger logger)
        {
            logger.LogInformation("Seeding demo users...");

            // Demo Dealers
            var demoUsers = new[]
            {
                new { Username = "dealer1", Email = "dealer1@topdrivex.bg", Password = "Dealer123!", FirstName = "Иван", LastName = "Петров", Type = UserType.Dealer, Role = "Dealer", City = "София", Dealership = "Авто Център София" },
                new { Username = "dealer2", Email = "dealer2@topdrivex.bg", Password = "Dealer123!", FirstName = "Мария", LastName = "Георгиева", Type = UserType.Dealer, Role = "Dealer", City = "Пловдив", Dealership = "Авто Трейд Пловдив" },
                
                // Demo Private Users
                new { Username = "user1", Email = "user1@topdrivex.bg", Password = "User123!", FirstName = "Георги", LastName = "Димитров", Type = UserType.Private, Role = "Private", City = "Варна", Dealership = (string?)null },
                new { Username = "user2", Email = "user2@topdrivex.bg", Password = "User123!", FirstName = "Елена", LastName = "Иванова", Type = UserType.Private, Role = "Private", City = "Бургас", Dealership = (string?)null },
                new { Username = "user3", Email = "user3@topdrivex.bg", Password = "User123!", FirstName = "Стефан", LastName = "Тодоров", Type = UserType.Private, Role = "Private", City = "Русе", Dealership = (string?)null },
            };

            int createdCount = 0;

            foreach (var demoUser in demoUsers)
            {
                // Check if user exists
                var existingUser = await userManager.FindByEmailAsync(demoUser.Email);

                if (existingUser == null)
                {
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = demoUser.Username,
                        Email = demoUser.Email,
                        FirstName = demoUser.FirstName,
                        LastName = demoUser.LastName,
                        PhoneNumber = $"+359{new Random().Next(880000000, 899999999)}",
                        UserType = demoUser.Type,
                        City = demoUser.City,
                        DealershipName = demoUser.Dealership,
                        Country = "Bulgaria",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, demoUser.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, demoUser.Role);
                        createdCount++;
                        logger.LogInformation($"✅ Demo user created: {demoUser.Email} ({demoUser.Role})");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"❌ Failed to create demo user {demoUser.Email}: {errors}");
                    }
                }
            }

            logger.LogInformation($"Demo users seeded: {createdCount} / {demoUsers.Length}");
        }
    }
}
