using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiverController : ControllerBase
    {
        private readonly DiverService _diverService;
        private readonly JWTService _jwtService;

        public DiverController(DiverService diverService, JWTService jwtService)
        {
            _diverService = diverService;
            _jwtService = jwtService;
        }
        [Authorize]
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

        [HttpGet("FetchHistoricSeaDataByLocationHTML")]
        public async Task<IActionResult> FetchHistoricSeaDataByLocationHTML([FromQuery] string location)
        {
            try
            {
                var result = await _diverService.FetchHistoricSeaDataByLocationHTML(location);
                return Ok(new { Message = "Historic HTML sea data fetched successfully.", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("FetchHistoricSeaDataByLocationGif")]
        public async Task<IActionResult> FetchHistoricSeaDataByLocationGif([FromQuery] string location)
        {
            try
            {
                var result = await _diverService.FetchHistoricSeaDataByLocationGif(location);
                return Ok(new { Message = "Historic GIF sea data fetched successfully.", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("FetchHistoricSeaDataByLocationStorm")]
        public async Task<IActionResult> FetchHistoricSeaDataByLocationStorm([FromQuery] string location)
        {
            try
            {
                var result = await _diverService.FetchHistoricSeaDataByLocationStorm(location);
                return Ok(new { Message = "Historic storm sea data fetched successfully.", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("FetchIndexSeaDataByPeriod")]
        public async Task<IActionResult> FetchIndexSeaDataByPeriod([FromQuery] int period)
        {
            try
            {
                var result = await _diverService.FetchIndexSeaData(period);
                return Ok(new { Message = "Sea Index fetched successfully.", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
