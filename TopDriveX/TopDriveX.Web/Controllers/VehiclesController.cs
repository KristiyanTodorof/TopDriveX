using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Dtos;
using TopDriveX.Domain.Enums;
using TopDriveX.Domain.Models;

namespace TopDriveX.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMakeService _makeService;
        private readonly IModelService _modelService;
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly UserManager<User> _userManager;

        public VehiclesController(
            IVehicleService vehicleService,
            IMakeService makeService,
            IModelService modelService,
            IVehicleTypeService vehicleTypeService,
            UserManager<User> userManager)
        {
            _vehicleService = vehicleService;
            _makeService = makeService;
            _modelService = modelService;
            _vehicleTypeService = vehicleTypeService;
            _userManager = userManager;
        }

        // ==================== INDEX ====================

        [HttpGet]
        public async Task<IActionResult> Index(
           Guid? makeId, Guid? modelId,
           int? yearFrom, int? yearTo,
           decimal? priceFrom, decimal? priceTo,
           int? mileageFrom, int? mileageTo,
           string? city,
           int page = 1,
           int pageSize = 12)
        {
            var allFilteredVehicles = await _vehicleService.SearchVehiclesAsync(
                makeId, modelId, yearFrom, yearTo,
                priceFrom, priceTo, mileageFrom, mileageTo, city);

            var totalVehicles = allFilteredVehicles.Count();

            var paginatedVehicles = allFilteredVehicles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var makes = await _makeService.GetAllMakesAsync();
            var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypesAsync();

            ViewBag.Makes = new SelectList(makes, "Id", "Name", makeId);
            ViewBag.VehicleTypes = new SelectList(vehicleTypes, "Id", "Name");

            ViewBag.TotalVehicles = totalVehicles;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.MakeId = makeId;
            ViewBag.ModelId = modelId;
            ViewBag.YearFrom = yearFrom;
            ViewBag.YearTo = yearTo;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;
            ViewBag.City = city;

            return View(paginatedVehicles);
        }

        // ==================== DETAILS ====================

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // ==================== CREATE GET ====================

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();

            var model = new CreateAdvertisementDto();
            return View(model);
        }

        // ==================== CREATE POST ====================

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdvertisementDto model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();

            try
            {
                // ViewModel → DTO → Service
                var dto = model.ToDto();
                var adId = await _vehicleService.CreateAdvertisementAsync(dto, Guid.Parse(userIdString));

                TempData["Success"] = "Обявата беше публикувана успешно!";
                return RedirectToAction(nameof(Details), new { id = adId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Възникна грешка при публикуването. Моля опитайте отново.");
                await PopulateDropdownsAsync();
                return View(model);
            }
        }

        // ==================== AJAX: MODELS BY MAKE ====================

        [HttpGet]
        public async Task<IActionResult> GetModelsByMake(Guid makeId)
        {
            var models = await _modelService.GetModelsByMakeIdAsync(makeId);
            return Json(models.Select(m => new { id = m.Id, name = m.Name }));
        }

        // ==================== AJAX ENDPOINTS ====================

        [HttpGet]
        public async Task<IActionResult> Search(
            Guid? makeId, Guid? modelId,
            int? yearFrom, int? yearTo,
            decimal? priceFrom, decimal? priceTo,
            int? mileageFrom, int? mileageTo,
            string? city)
        {
            var vehicles = await _vehicleService.SearchVehiclesAsync(
                makeId, modelId, yearFrom, yearTo,
                priceFrom, priceTo, mileageFrom, mileageTo, city);

            return Json(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Featured(int count = 6)
        {
            var vehicles = await _vehicleService.GetFeaturedVehiclesAsync(count);
            return Json(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Latest(int count = 10)
        {
            var vehicles = await _vehicleService.GetLatestVehiclesAsync(count);
            return Json(vehicles);
        }

        // ==================== HELPERS ====================

        private async Task PopulateDropdownsAsync()
        {
            var makes = await _makeService.GetAllMakesAsync();
            var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypesAsync();

            ViewBag.Makes = new SelectList(makes, "Id", "Name");
            ViewBag.VehicleTypes = new SelectList(vehicleTypes, "Id", "Name");

            ViewBag.FuelTypes = new SelectList(new[]
            {
                new { Value = FuelType.Petrol.ToString(),       Text = "Бензин" },
                new { Value = FuelType.Diesel.ToString(),       Text = "Дизел" },
                new { Value = FuelType.Electric.ToString(),     Text = "Електричество" },
                new { Value = FuelType.Hybrid.ToString(),       Text = "Хибрид" },
                new { Value = FuelType.PlugInHybrid.ToString(), Text = "Plug-in хибрид" },
                new { Value = FuelType.LPG.ToString(),          Text = "ГАЗ (LPG)" },
                new { Value = FuelType.CNG.ToString(),          Text = "CNG" },
            }, "Value", "Text");

            ViewBag.TransmissionTypes = new SelectList(new[]
            {
                new { Value = TransmissionType.Manual.ToString(),        Text = "Ръчна" },
                new { Value = TransmissionType.Automatic.ToString(),     Text = "Автоматична" },
                new { Value = TransmissionType.SemiAutomatic.ToString(), Text = "Полуавтоматична" },
                new { Value = TransmissionType.CVT.ToString(),           Text = "CVT" },
            }, "Value", "Text");

            ViewBag.BodyStyles = new SelectList(new[]
            {
                new { Value = BodyStyle.Sedan.ToString(),       Text = "Седан" },
                new { Value = BodyStyle.Hatchback.ToString(),   Text = "Хечбек" },
                new { Value = BodyStyle.SUV.ToString(),         Text = "SUV" },
                new { Value = BodyStyle.Coupe.ToString(),       Text = "Купе" },
                new { Value = BodyStyle.Convertible.ToString(), Text = "Кабриолет" },
                new { Value = BodyStyle.Wagon.ToString(),       Text = "Комби" },
                new { Value = BodyStyle.Van.ToString(),         Text = "Ван" },
                new { Value = BodyStyle.Pickup.ToString(),      Text = "Пикап" },
                new { Value = BodyStyle.Minivan.ToString(),     Text = "Миниван" },
                new { Value = BodyStyle.Crossover.ToString(),   Text = "Кросоувър" },
            }, "Value", "Text");

            ViewBag.Colors = new SelectList(new[]
            {
                "Черен", "Бял", "Сребрист", "Сив", "Червен", "Син",
                "Зелен", "Жълт", "Оранжев", "Кафяв", "Бежов", "Лилав", "Друг"
            });
        }
    }
}
