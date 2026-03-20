using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.AspNetCore.Http;

namespace project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthApiController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _userService.CreateAsync(model);
            if (!created)
            {
                return BadRequest("User registration failed.");
            }

            return Ok(model);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.AuthenticateAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.SetString("UserEmail", user.Email);
            }

            return Ok(user);
        }

        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.Clear();
            }

            return Ok(new { message = "Logged out" });
        }
    }
}
