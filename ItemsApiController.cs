using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsApiController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsApiController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }
    }
}