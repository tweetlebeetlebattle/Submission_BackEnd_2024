﻿using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeightlifterBlogController : ControllerBase
    {
        private readonly WeightlifterService _weightlifterService;
        private readonly JWTService _jwtService;

        public WeightlifterBlogController(WeightlifterService weightlifterService, JWTService jwtService)
        {
            _weightlifterService = weightlifterService;
            _jwtService = jwtService;
        }
        [Authorize]
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
        [Authorize]
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
        public async Task<IActionResult> FetchAllApprovedComments([FromQuery] int blogsPerPage, [FromQuery] int pageNumber)
        {
            try
            {
                if (blogsPerPage <= 0 || pageNumber <= 0)
                {
                    return BadRequest(new { Message = "blogsPerPage and pageNumber must be greater than 0." });
                }

                int totalBlogs = await _weightlifterService.FetchNumberOfBlogs();

                int totalPages = (int)Math.Ceiling((double)totalBlogs / blogsPerPage);

                if (pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }

                int skip = (pageNumber - 1) * blogsPerPage;

                var result = await _weightlifterService.FetchAllApprovedCommentsAsync(skip, blogsPerPage);

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
        [HttpGet("FetchNumberOfApprovedWeightlifterBlogs")]
        public async Task<IActionResult> FetchNumberOfApprovedWeightlifterBlogs()
        {
            try
            {
                var result = await _weightlifterService.FetchNumberOfBlogs();

                return Ok(new { Message = "Approved blogs counted successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("FetchNumberOfApprovedUserWeightlifterBlogs")]
        public async Task<IActionResult> FetchNumberOfApprovedUserWeightlifterBlogs([FromQuery] SearchSuggestions query)
        {
            try
            {
                var result = await _weightlifterService.FetchNumberOfApprovedUserWeightlifterBlogs(query.SearchQuery);

                return Ok(new { Message = "Approved blogs counted successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("FetchApprovedUserWeightlifterBlogs")]
        public async Task<IActionResult> FetchApprovedUserWeightlifterBlogs([FromQuery] int blogsPerPage, [FromQuery] int pageNumber, [FromQuery] string username)
        {
            try
            {
                int totalBlogs = await _weightlifterService.FetchNumberOfApprovedUserWeightlifterBlogs(username);

                int totalPages = (int)Math.Ceiling((double)totalBlogs / blogsPerPage);

                if (pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }

                int skip = (pageNumber - 1) * blogsPerPage;

                var result = await _weightlifterService.FetchApprovedUserWeightlifterBlogs(skip, blogsPerPage, username);

                return Ok(new { Message = "Approved blogs fetched successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
