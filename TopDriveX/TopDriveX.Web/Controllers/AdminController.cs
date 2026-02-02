using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopDriveX.Domain.Models;
using TopDriveX.Infrastructure.Data;

namespace TopDriveX.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<User> userManager,
            ApplicationDbContext context,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // ==================== DASHBOARD ====================

        /// <summary>
        /// GET: /Admin
        /// Admin dashboard with statistics
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Get statistics
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalMakes = await _context.Makes.CountAsync(),
                TotalModels = await _context.Models.CountAsync(),
                TotalVehicles = await _context.Vehicles.CountAsync(),
                TotalAdvertisements = await _context.Advertisements.CountAsync(),
                ActiveAdvertisements = await _context.Advertisements
                    .Where(a => a.Status == Domain.Enums.AdvertisementStatus.Active)
                    .CountAsync()
            };

            ViewBag.Stats = stats;

            return View();
        }

        // ==================== USERS MANAGEMENT ====================

        /// <summary>
        /// GET: /Admin/Users
        /// List all users
        /// </summary>
        public async Task<IActionResult> Users(int page = 1, int pageSize = 20)
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(users);
        }

        /// <summary>
        /// GET: /Admin/UserDetails/{id}
        /// View user details
        /// </summary>
        public async Task<IActionResult> UserDetails(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Advertisements)
                .Include(u => u.Favorites)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// POST: /Admin/ToggleUserStatus/{id}
        /// Activate/Deactivate user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Потребителят {user.UserName} беше {(user.IsActive ? "активиран" : "деактивиран")}";

            return RedirectToAction(nameof(Users));
        }

        // ==================== ADVERTISEMENTS MANAGEMENT ====================

        /// <summary>
        /// GET: /Admin/Advertisements
        /// List all advertisements
        /// </summary>
        public async Task<IActionResult> Advertisements(int page = 1, int pageSize = 20)
        {
            var ads = await _context.Advertisements
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.Make)
                .Include(a => a.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalAds = await _context.Advertisements.CountAsync();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(ads);
        }

        /// <summary>
        /// POST: /Admin/ApproveAd/{id}
        /// Approve pending advertisement
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAd(Guid id)
        {
            var ad = await _context.Advertisements.FindAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            ad.Status = Domain.Enums.AdvertisementStatus.Active;
            ad.PublishedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Обявата беше одобрена";

            return RedirectToAction(nameof(Advertisements));
        }

        /// <summary>
        /// POST: /Admin/DeleteAd/{id}
        /// Soft delete advertisement
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAd(Guid id)
        {
            var ad = await _context.Advertisements.FindAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            ad.IsDeleted = true;
            ad.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Обявата беше изтрита";

            return RedirectToAction(nameof(Advertisements));
        }

        // ==================== MAKES MANAGEMENT ====================

        /// <summary>
        /// GET: /Admin/Makes
        /// List all makes
        /// </summary>
        public async Task<IActionResult> Makes()
        {
            var makes = await _context.Makes
                .Include(m => m.Models)
                .OrderBy(m => m.Name)
                .ToListAsync();

            return View(makes);
        }

        // ==================== MODELS MANAGEMENT ====================

        /// <summary>
        /// GET: /Admin/Models
        /// List all models
        /// </summary>
        public async Task<IActionResult> Models()
        {
            var models = await _context.Models
                .Include(m => m.Make)
                .OrderBy(m => m.Make.Name)
                .ThenBy(m => m.Name)
                .ToListAsync();

            return View(models);
        }

        // ==================== SETTINGS ====================

        /// <summary>
        /// GET: /Admin/Settings
        /// System settings
        /// </summary>
        public IActionResult Settings()
        {
            return View();
        }
    }
}
