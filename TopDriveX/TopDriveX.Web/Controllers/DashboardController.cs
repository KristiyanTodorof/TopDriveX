using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;
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

        // ==================== INDEX (Default) ====================
        // Redirect /Dashboard to /Dashboard/Profile

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profile));
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

            var model = new UserSettingsDto
            {
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName ?? "",
                UserType = user.UserType.ToString(),
                CreatedAt = user.CreatedAt,

                EmailNotificationsMessages = true,
                EmailNotificationsComments = true,
                EmailNotificationsPriceChanges = true,
                SmsNotifications = false,
                ShowPhonePublicly = true,
                ShowEmailPublicly = false,
                AllowDirectMessages = true
            };

            return View(model);
        }

        // ==================== SETTINGS POST ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(UserSettingsDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Настройките бяха запазени успешно!";
                return RedirectToAction(nameof(Settings));
            }
            else
            {
                TempData["Error"] = "Възникна грешка при запазването!";
                ModelState.AddModelError("", "Не успяхме да запазим промените.");
                return View(model);
            }
        }

        // ==================== CHANGE PASSWORD ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                TempData["Error"] = "Моля попълнете всички полета!";
                return RedirectToAction(nameof(Settings));
            }

            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "Новата парола и потвърждението не съвпадат!";
                return RedirectToAction(nameof(Settings));
            }

            if (newPassword.Length < 6)
            {
                TempData["Error"] = "Паролата трябва да е поне 6 символа!";
                return RedirectToAction(nameof(Settings));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Паролата беше променена успешно!";
            }
            else
            {
                TempData["Error"] = "Грешка при смяна на паролата. Проверете текущата парола.";
            }

            return RedirectToAction(nameof(Settings));
        }

        // ==================== DELETE ACCOUNT ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordCheck)
            {
                TempData["Error"] = "Грешна парола!";
                return RedirectToAction(nameof(Settings));
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await HttpContext.SignOutAsync();
                TempData["Success"] = "Акаунтът беше изтрит успешно.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Възникна грешка при изтриването на акаунта!";
                return RedirectToAction(nameof(Settings));
            }
        }
    }
}
