using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TopDriveX.Application.Contracts;

namespace TopDriveX.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMakeService _makeService;
        private readonly IVehicleTypeService _vehicleTypeService;

        public HomeController(IMakeService makeService, IVehicleTypeService vehicleTypeService)
        {
            _makeService = makeService;
            _vehicleTypeService = vehicleTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var makes = await _makeService.GetAllMakesAsync();
            var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypesAsync();

            ViewBag.VehicleTypes = vehicleTypes;
            return View(makes);
        }

        public IActionResult Search(Guid? makeId, Guid? vehicleTypeId, decimal? minPrice, decimal? maxPrice)
        {
            // TODO: Implement search logic
            TempData["SearchMessage"] = "Търсенето работи! Скоро ще добавим реални резултати.";
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
