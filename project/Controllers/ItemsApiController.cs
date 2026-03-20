using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;

namespace project.Controllers
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
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }
    }
}