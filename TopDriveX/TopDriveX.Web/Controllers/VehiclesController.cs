using Microsoft.AspNetCore.Mvc;
using TopDriveX.Application.Contracts;

namespace TopDriveX.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Search vehicles with filters (AJAX endpoint)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Search(
            Guid? makeId,
            Guid? modelId,
            int? yearFrom,
            int? yearTo,
            decimal? priceFrom,
            decimal? priceTo,
            int? mileageFrom,
            int? mileageTo,
            string? city)
        {
            var vehicles = await _vehicleService.SearchVehiclesAsync(
                makeId, modelId, yearFrom, yearTo,
                priceFrom, priceTo, mileageFrom, mileageTo, city);

            return Json(vehicles);
        }

        /// <summary>
        /// Get all vehicles with filters (Page)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? makeId,
            Guid? modelId,
            int? yearFrom,
            int? yearTo,
            decimal? priceFrom,
            decimal? priceTo,
            string? city)
        {
            var vehicles = await _vehicleService.SearchVehiclesAsync(
                makeId, modelId, yearFrom, yearTo,
                priceFrom, priceTo, null, null, city);

            // Pass filter data to view
            ViewBag.MakeId = makeId;
            ViewBag.ModelId = modelId;
            ViewBag.YearFrom = yearFrom;
            ViewBag.YearTo = yearTo;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;
            ViewBag.City = city;

            return View(vehicles);
        }

        /// <summary>
        /// Get vehicle details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        /// <summary>
        /// Get featured vehicles (AJAX endpoint)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Featured(int count = 6)
        {
            var vehicles = await _vehicleService.GetFeaturedVehiclesAsync(count);
            return Json(vehicles);
        }

        /// <summary>
        /// Get latest vehicles (AJAX endpoint)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Latest(int count = 10)
        {
            var vehicles = await _vehicleService.GetLatestVehiclesAsync(count);
            return Json(vehicles);
        }
    }
}
