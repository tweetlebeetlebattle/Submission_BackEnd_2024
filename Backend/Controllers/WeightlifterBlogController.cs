using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protect all endpoints with JWT authentication
    public class WeightlifterBlogController : ControllerBase
    {
        private readonly WeightlifterService _weightlifterService;
        private readonly JWTService _jwtService;
        private readonly UtilityService _utilityService;

        public WeightlifterBlogController(WeightlifterService weightlifterService, JWTService jwtService, UtilityService utilityService)
        {
            _weightlifterService = weightlifterService;
            _jwtService = jwtService;
            _utilityService = utilityService;
        }

        private async Task<string> GetUserIdFromJwtAsync()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Authorization token is missing.");
            }

            var email = _jwtService.RetrieveEmailFromToken(token);
            Console.WriteLine(email); // nothing is retrieved?? TokenToEmail does not work
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }

            var userId = await _utilityService.GetUserIdByEmailAsync(email);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            return userId;
        }

        [HttpPost("CreateNewBlog")]
        public async Task<IActionResult> CreateNewBlog([FromForm] CreateNewBlogDto dto)
        {
            var userId = await GetUserIdFromJwtAsync(); // Fetch user ID from token
            var result = await _weightlifterService.CreateNewBlogAsync(dto.Text, dto.Image, userId, dto.DateTimestamp);
            return Ok(result);
        }

        [HttpPost("CreateNewComment")]
        public async Task<IActionResult> CreateNewComment([FromForm] CreateNewCommentDto dto)
        {
            var userId = await GetUserIdFromJwtAsync(); // Fetch user ID from token
            var result = await _weightlifterService.CreateNewCommentAsync(dto.BlogId, dto.Text, dto.Image, userId, dto.DateTimestamp);
            return Ok(result);
        }

        [HttpGet("FetchAllApprovedComments")]
        public async Task<IActionResult> FetchAllApprovedComments()
        {
            var result = await _weightlifterService.FetchAllApprovedCommentsAsync();
            return Ok(result);
        }
    }
}
