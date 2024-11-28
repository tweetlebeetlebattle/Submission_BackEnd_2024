using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class DiverController : ControllerBase
    {
        private readonly DiverService _diverService;
        private readonly JWTService _jwtService;

        public DiverController(DiverService diverService, JWTService jwtService)
        {
            _diverService = diverService;
            _jwtService = jwtService;
        }

        [HttpPost("PostUserFeedback")]
        public async Task<IActionResult> PostUserFeedback([FromForm] FeedbackDTO feedbackDto)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                await _diverService.PostUserFeedback(userId, feedbackDto);

                return Ok(new { Message = "User feedback submitted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
