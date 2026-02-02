using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TopDriveX.Domain.Models;

namespace TopDriveX.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            UserManager<User> userManager,
            ILogger<DashboardController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // ==================== DASHBOARD HOME ====================

        /// <summary>
        /// GET: /Dashboard
        /// Main dashboard page
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserFullName = $"{user.FirstName} {user.LastName}";
            ViewBag.UserType = user.UserType.ToString();

            return View(user);
        }

        // ==================== MY ADVERTISEMENTS ====================

        /// <summary>
        /// GET: /Dashboard/MyAds
        /// Shows user's advertisements
        /// </summary>
        public async Task<IActionResult> MyAds()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // TODO: Fetch user's advertisements from database
            // var ads = await _advertisementService.GetUserAdvertisementsAsync(user.Id);

            return View();
        }

        // ==================== FAVORITES ====================

        /// <summary>
        /// GET: /Dashboard/Favorites
        /// Shows user's favorite advertisements
        /// </summary>
        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // TODO: Fetch user's favorites
            // var favorites = await _favoriteService.GetUserFavoritesAsync(user.Id);

            return View();
        }

        // ==================== PROFILE ====================

        /// <summary>
        /// GET: /Dashboard/Profile
        /// User profile page
        /// </summary>
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        // ==================== SETTINGS ====================

        /// <summary>
        /// GET: /Dashboard/Settings
        /// Account settings
        /// </summary>
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
    }
}
