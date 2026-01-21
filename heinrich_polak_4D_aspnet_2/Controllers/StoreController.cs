using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;

namespace DominikPatakovASP.Controllers
{
    public class StoreController : Controller
    {
        private readonly IItemService _itemService;

        public StoreController(IItemService itemService)
        {
            _itemService = itemService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemService.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }
    }
}
