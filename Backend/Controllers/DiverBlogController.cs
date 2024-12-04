using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiverBlogController : ControllerBase
    {
        private readonly DiverService _diverService;
        private readonly JWTService _jwtService;

        public DiverBlogController(DiverService diverService, JWTService jwtService)
        {
            _diverService = diverService;
            _jwtService = jwtService;
        }
        [Authorize]
        [HttpPost("CreateNewBlog")]
        public async Task<IActionResult> CreateNewBlog([FromForm] CreateNewBlogDto dto)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                var result = await _diverService.CreateNewBlogAsync(dto.Text, dto.Image, userId);

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
        [Authorize]
        [HttpPost("CreateNewComment")]
        public async Task<IActionResult> CreateNewComment([FromForm] CreateNewCommentDto dto)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                var result = await _diverService.CreateNewCommentAsync(dto.BlogId, dto.Text, dto.Image, userId);

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
                var result = await _diverService.FetchAllApprovedCommentsAsync();

                return Ok(new { Message = "Approved comments fetched successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("FetchNumberOfApprovedBlogs")]
        public async Task<IActionResult> FetchNumberOfApprovedBlogs()
        {
            try
            {
                var result = await _diverService.FetchNumberOfBlogs();

                return Ok(new { Message = "Approved blogs counted successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
