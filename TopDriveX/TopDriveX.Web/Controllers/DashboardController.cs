using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TopDriveX.Application.Contracts;
using TopDriveX.Domain.Models;

namespace TopDriveX.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly UserManager<User> _userManager;

        public DashboardController(
            IVehicleService vehicleService,
            UserManager<User> userManager)
        {
            _vehicleService = vehicleService;
            _userManager = userManager;
        }

        // ==================== MY ADS ====================

        [HttpGet]
        public async Task<IActionResult> MyAds()
        {
            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();

            var userId = Guid.Parse(userIdString);
            var myAds = await _vehicleService.GetUserAdvertisementsAsync(userId);

            return View(myAds);
        }

        // ==================== FAVORITES ====================

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();

            // TODO: Implement favorites service
            var favorites = new List<object>();

            return View(favorites);
        }

        // ==================== PROFILE ====================

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var userId = Guid.Parse(_userManager.GetUserId(User)!);
            var stats = await _vehicleService.GetUserStatsAsync(userId);

            ViewBag.ActiveAds = stats.activeAds;
            ViewBag.TotalViews = stats.totalViews;
            ViewBag.TotalFavorites = stats.totalFavorites;

            return View(user);
        }

        // ==================== SETTINGS GET ====================

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            return View(user);
        }

        // ==================== SETTINGS POST ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(User model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Update only allowed fields
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Настройките бяха запазени успешно!";
            }
            else
            {
                TempData["Error"] = "Възникна грешка при запазването!";
            }

            return RedirectToAction(nameof(Settings));
        }
    }
}
