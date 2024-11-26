using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Data.ExtendedModel;
using Backend.DTO.RequestResponseDTOs.Auth;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTService _jwtService;

        public AuthController(UserManager<IdentityUser> userManager, JWTService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = _jwtService.GenerateJwtTokenByEmail(user.Email);
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin"); 
                return Ok(new AuthenticationSuccessResponse { Username = user.UserName, Token = token, IsAdmin = isAdmin });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = _jwtService.GenerateJwtTokenByEmail(user.Email);
                return Ok(new AuthenticationSuccessResponse { Username = user.UserName, Token = token, IsAdmin = false });
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { error = errors });
            }
        }
    }
}
