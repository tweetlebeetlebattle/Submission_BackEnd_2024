
using Backend.DTO.RequestResponseDTOs.Admin;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("FetchAllUnapprovedBlogComments")]
        public async Task<IActionResult> FetchAllUnapprovedBlogComments()
        {
            var result = await _adminService.FetchAllUnapprovedBlogCommentsAsync();
            return Ok(result);
        }

        [HttpPost("ApproveOrRejectBlogComment")]
        public async Task<IActionResult> ApproveOrRejectBlogComment([FromBody] ApproveRejectRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Invalid request. ID and status are required.");

            if (dto.Status != "approved" && dto.Status != "rejected")
                return BadRequest("Status must be either 'approved' or 'rejected'.");

            var result = await _adminService.ApproveOrRejectBlogCommentAsync(dto.Id, dto.Status);
            return Ok(result);
        }
    }
}
