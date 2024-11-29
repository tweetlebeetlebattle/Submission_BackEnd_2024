
using Backend.DTO.RequestResponseDTOs.Admin;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService adminService;

        public AdminController(AdminService _adminService)
        {
            adminService = _adminService;
        }

        [HttpGet("FetchAllUnapprovedBlogComments")]
        public async Task<IActionResult> FetchAllUnapprovedBlogComments()
        {
            var result = await adminService.FetchAllUnapprovedBlogCommentsAsync();
            return Ok(result);
        }

        [HttpPost("ApproveOrRejectBlogComment")]
        public async Task<IActionResult> ApproveOrRejectBlogComment([FromBody] ApproveRejectRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Invalid request. ID and status are required.");

            if (dto.Status != "approved" && dto.Status != "rejected")
                return BadRequest("Status must be either 'approved' or 'rejected'.");

            var result = await adminService.ApproveOrRejectBlogCommentAsync(dto.Id, dto.Status);
            return Ok(result);
        }

        [HttpGet("FetchAllFeedbacks")]
        public async Task<IActionResult> FetchAllFeedbacks()
        {
            var result = await adminService.FetchAllFeedbacks();
            return Ok(result);
        }
        [HttpPost("DeleteFeedback")]
        public async Task<IActionResult> DeleteFeedback([FromBody] DeleteFeedback deleteFeedback)
        {
            var result = await adminService.DeleteFeedback(deleteFeedback.Id);
            return Ok(result);
        }
    }
}
