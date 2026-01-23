using Microsoft.AspNetCore.Mvc;
using TopDriveX.Application.Contracts;

namespace TopDriveX.Web.Controllers
{
    public class MakesController : Controller
    {
        private readonly IMakeService _makeService;
        private readonly IModelService _modelService;

        public MakesController(IMakeService makeService, IModelService modelService)
        {
            _makeService = makeService;
            _modelService = modelService;
        }

        public async Task<IActionResult> Index()
        {
            var makes = await _makeService.GetAllMakesAsync();
            return View(makes);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var make = await _makeService.GetMakeByIdAsync(id);
            if (make == null)
                return NotFound();

            var models = await _modelService.GetModelsByMakeIdAsync(id);

            ViewBag.Models = models;
            return View(make);
        }
    }
}
