﻿using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.DTO.RequestResponseDTOs.Weightlifter;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeightlifterController : ControllerBase
    {
        private readonly WeightlifterService _weightlifterService;
        private readonly JWTService _jwtService;

        public WeightlifterController(WeightlifterService diverService, JWTService jwtService)
        {
            _weightlifterService = diverService;
            _jwtService = jwtService;
        }
        [Authorize]
        [HttpGet("FetchAllUserTrainingAndUniversalLogs")]
        public async Task<IActionResult> FetchAllUserTrainingAndUniversalLogs()
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);
                var result = await _weightlifterService.FetchAllUserTrainingAndUniversalLogs(userId);

                return Ok(new { Message = "Training and universal logs retrieved successfully.", Data = result });
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

                return Ok(new { Message = "Public user data retrieved successfully.", Data = result });
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

                return Ok(new { Message = "Training units retrieved successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("CreateNewUniversalReading")]
        [Authorize]
        public async Task<IActionResult> CreateNewUniversalReading([FromBody] CreateUniversalReading createUniversalReading)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);
                await _weightlifterService.CreateNewUniversalReading(userId, createUniversalReading);

                return Ok(new { Message = "New Universal Reading created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("CreateNewTraining")]
        [Authorize]
        public async Task<IActionResult> CreateNewTraining([FromForm] CreateNewTraining createNewTraining)
        {
            try
            {
                var userId = await _jwtService.GetUserIdFromJwtAsync(Request.Headers["Authorization"]);
                await _weightlifterService.CreateNewTraining(userId, createNewTraining);

                return Ok(new { Message = "New Training Log created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("UpdateUniversalReadingPublicity")]
        [Authorize]
        public async Task<IActionResult> UpdateUniversalReadingPublicity([FromBody] ChangeUniversalReadingTrainingPublicity changeForm)
        {
            try
            {
                await _weightlifterService.UpdateUniversalReadingPublicity(changeForm);

                return Ok(new { Message = "Universal Reading publicity updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("UpdateTrainingLogPublicity")]
        [Authorize]
        public async Task<IActionResult> UpdateTrainingLogPublicity([FromBody] ChangeUniversalReadingTrainingPublicity changeForm)
        {
            try
            {
                await _weightlifterService.UpdateTrainingLogPublicity(changeForm);

                return Ok(new { Message = "Training Log publicity updated successfully." });
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

                return Ok(new { Message = "Training titles retrieved successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
