using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.DTO.RequestResponseDTOs.Weightlifter;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeightlifterController : ControllerBase
    {
        private readonly WeightlifterService _weightlifterService;
        private readonly JWTService _jwtService;

        public WeightlifterController(WeightlifterService diverService, JWTService jwtService)
        {
            _weightlifterService = diverService;
            _jwtService = jwtService;
        }

        [HttpGet("FetchAllUserTrainingAndUniversalLogs")]
        public async Task<IActionResult> FetchAllUserTrainingAndUniversalLogs()
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                var result = await _weightlifterService.FetchAllUserTrainingAndUniversalLogs(userId);

                return Ok(new { result });
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
        [HttpGet("FetchPublicUserData")]
        public async Task<IActionResult> FetchPublicUserData([FromQuery] string Username)
        {
            try
            {
                var result = await _weightlifterService.FetchPublicUserData(Username);

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("FetchAllTrainingUnits")]
        public async Task<IActionResult> FetchAllTrainingUnits()
        {
            try
            {
                var result = await _weightlifterService.FetchAllTrainingUnits();

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpPost("CreateNewUniversalReading")]
        public async Task<IActionResult> CreateNewUniversalReading([FromBody] CreateUniversalReading createUniversalReading)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                await _weightlifterService.CreateNewUniversalReading(userId, createUniversalReading);

                return Ok(new { Message = "New Universal Reading created." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpPost("CreateNewTraining")]
        public async Task<IActionResult> CreateNewTraining([FromForm] CreateNewTraining createNewTraining)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);

                await _weightlifterService.CreateNewTraining(userId, createNewTraining);

                return Ok(new { Message = "New Training Log created." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpPost("UpdateUniversalReadingPublicity")]
        public async Task<IActionResult> UpdateUniversalReadingPublicity([FromBody] ChangeUniversalReadingTrainingPublicity changeForm)
        {
            try
            {
                await _weightlifterService.UpdateUniversalReadingPublicity(changeForm);

                return Ok(new { Message = "Updated Universal Reading Publicity." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpPost("UpdateTrainingLogPublicity")]
        public async Task<IActionResult> UpdateTrainingLogPublicity([FromBody] ChangeUniversalReadingTrainingPublicity changeForm)
        {
            try
            {
                await _weightlifterService.UpdateTrainingLogPublicity(changeForm);

                return Ok(new { Message = "Updated Training Log Publicity." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("FetchAllTrainingTitles")]
        public async Task<IActionResult> FetchAllTrainingTitles()
        {
            try
            {
                var result = await _weightlifterService.FetchAllTrainingTitles();

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
