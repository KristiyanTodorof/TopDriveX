using Microsoft.AspNetCore.Mvc;
using TopDriveX.Application.Contracts;

namespace TopDriveX.Web.Controllers
{
    public class MakesController : Controller
    {
        private readonly IMakeService _makeService;

        public MakesController(IMakeService makeService)
        {
            _makeService = makeService;
        }

        public async Task<IActionResult> Index()
        {
            var makes = await _makeService.GetAllMakesAsync();
            return View(makes);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var makeDetails = await _makeService.GetMakeDetailsAsync(id);

            if (makeDetails == null)
                return NotFound();

            return View(makeDetails);
        }
    }
}
