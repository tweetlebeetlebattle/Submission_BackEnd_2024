using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class WeightlifterBlogController : ControllerBase
    {
        private readonly WeightlifterService _weightlifterService;
        private readonly JWTService _jwtService;

        public WeightlifterBlogController(WeightlifterService weightlifterService, JWTService jwtService)
        {
            _weightlifterService = weightlifterService;
            _jwtService = jwtService;
        }

        [HttpPost("CreateNewBlog")]
        public async Task<IActionResult> CreateNewBlog([FromForm] CreateNewBlogDto dto)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                var result = await _weightlifterService.CreateNewBlogAsync(dto.Text, dto.Image, userId);

                return Ok(new { Message = "Blog created successfully.", Data = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("CreateNewComment")]
        public async Task<IActionResult> CreateNewComment([FromForm] CreateNewCommentDto dto)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                var result = await _weightlifterService.CreateNewCommentAsync(dto.BlogId, dto.Text, dto.Image, userId);

                return Ok(new { Message = "Comment created successfully.", Data = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("FetchAllApprovedComments")]
        public async Task<IActionResult> FetchAllApprovedComments()
        {
            try
            {
                var result = await _weightlifterService.FetchAllApprovedCommentsAsync();

                return Ok(new { Message = "Approved comments fetched successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
