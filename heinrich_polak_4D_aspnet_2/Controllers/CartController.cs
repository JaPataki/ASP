using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;

namespace DominikPatakovASP.Controllers
{
    public class CartController : Controller
    {
        private readonly IShoppingCartService _cartService;

        public CartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _cartService.GetCartItemsAsync();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid itemId, int qty = 1)
        {
            await _cartService.AddToCartAsync(itemId, qty);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            return RedirectToAction("Index");
        }
    }
}
