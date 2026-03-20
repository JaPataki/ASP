using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;


namespace project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IItemService _itemService;

        public AdminApiController(IUserService userService, IItemService itemService)
        {
            _userService = userService;
            _itemService = itemService;
        }

        
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("users/{publicId}")]
        public async Task<IActionResult> GetUserByPublicId(string publicId)
        {
            if (!Guid.TryParse(publicId, out var id))
            {
                return BadRequest("Invalid publicId format. Expected a GUID.");
            }

            var user = await _userService.GetByPublicIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("users/{publicId}")]
        public async Task<IActionResult> DeleteUser(string publicId)
        {
            if (!Guid.TryParse(publicId, out var id))
            {
                return BadRequest("Invalid publicId format. Expected a GUID.");
            }

            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("User delete failed.");
            }

            return NoContent();
        }

        
        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        
        [HttpGet("items/{itemId:guid}")]
        public async Task<IActionResult> GetItemById(Guid itemId)
        {
            var item = await _itemService.GetByIdAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
