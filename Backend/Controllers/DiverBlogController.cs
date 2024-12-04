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
        public async Task<IActionResult> FetchAllApprovedComments([FromQuery] int blogsPerPage, [FromQuery] int pageNumber)
        {
            try
            {
                if (blogsPerPage <= 0 || pageNumber <= 0)
                {
                    return BadRequest(new { Message = "blogsPerPage and pageNumber must be greater than 0." });
                }

                int totalBlogs = await _diverService.FetchNumberOfBlogs();

                int totalPages = (int)Math.Ceiling((double)totalBlogs / blogsPerPage);

                if (pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }

                int skip = (pageNumber - 1) * blogsPerPage;

                var result = await _diverService.FetchAllApprovedCommentsAsync(skip, blogsPerPage);

                return Ok(new
                {
                    Message = "Approved comments fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("FetchNumberOfApprovedDiverBlogs")]
        public async Task<IActionResult> FetchNumberOfApprovedDiverBlogs()
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
