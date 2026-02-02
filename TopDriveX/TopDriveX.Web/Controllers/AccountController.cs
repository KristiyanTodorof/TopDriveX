using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TopDriveX.Application.Dtos;
using TopDriveX.Domain.Enums;
using TopDriveX.Domain.Models;

namespace TopDriveX.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // ==================== REGISTER ====================

        /// <summary>
        /// GET: /Account/Register
        /// Shows registration form
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            // Redirect if already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        /// <summary>
        /// POST: /Account/Register
        /// Processes registration form
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            // Redirect if already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Parse UserType enum
            if (!Enum.TryParse<UserType>(model.UserType, out var userType))
            {
                ModelState.AddModelError("", "Невалиден тип потребител");
                return View(model);
            }

            // Create new user
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                UserType = userType,
                DealershipName = userType == UserType.Dealer ? model.DealershipName : null,
                DealershipAddress = userType == UserType.Dealer ? model.DealershipAddress : null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Create user with password
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Username} created a new account", user.UserName);

                // Assign role based on user type
                string roleName = userType == UserType.Dealer ? "Dealer" : "Private";
                await _userManager.AddToRoleAsync(user, roleName);

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["Success"] = "Регистрацията е успешна! Добре дошли в TopDriveX!";

                return RedirectToAction("Index", "Dashboard");
            }

            // Add errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // ==================== LOGIN ====================

        /// <summary>
        /// GET: /Account/Login
        /// Shows login form
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // Redirect if already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: /Account/Login
        /// Processes login form
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user by email or username
            var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail)
                    ?? await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Невалидно потребителско име или парола");
                return View(model);
            }

            // Check if user is active
            if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Вашият акаунт е деактивиран. Свържете се с поддръжката.");
                return View(model);
            }

            // Attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Username} logged in", user.UserName);

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                // Redirect to return URL or dashboard
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Username} account locked out", user.UserName);
                ModelState.AddModelError(string.Empty, "Вашият акаунт е заключен поради множество неуспешни опити за вход. Моля, опитайте отново след няколко минути.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Невалидно потребителско име или парола");
            return View(model);
        }

        // ==================== LOGOUT ====================

        /// <summary>
        /// POST: /Account/Logout
        /// Logs out the current user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");

            TempData["Success"] = "Излязохте успешно от профила си";

            return RedirectToAction("Index", "Home");
        }

        // ==================== ACCESS DENIED ====================

        /// <summary>
        /// GET: /Account/AccessDenied
        /// Shown when user tries to access forbidden resource
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
