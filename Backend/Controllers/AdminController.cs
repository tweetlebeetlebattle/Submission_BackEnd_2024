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
            try
            {
                var result = await adminService.FetchAllUnapprovedBlogCommentsAsync();
                return Ok(new { Message = "Unapproved blog comments fetched successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("ApproveOrRejectBlogComment")]
        public async Task<IActionResult> ApproveOrRejectBlogComment([FromBody] ApproveRejectRequestDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.Status))
                    return BadRequest(new { Message = "Invalid request. ID and status are required." });

                if (dto.Status != "approved" && dto.Status != "rejected")
                    return BadRequest(new { Message = "Status must be either 'approved' or 'rejected'." });

                var result = await adminService.ApproveOrRejectBlogCommentAsync(dto.Id, dto.Status);
                return Ok(new { Message = "Blog comment status updated successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("FetchAllFeedbacks")]
        public async Task<IActionResult> FetchAllFeedbacks()
        {
            try
            {
                var result = await adminService.FetchAllFeedbacks();
                return Ok(new { Message = "All feedbacks fetched successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("DeleteFeedback")]
        public async Task<IActionResult> DeleteFeedback([FromBody] DeleteFeedback deleteFeedback)
        {
            try
            {
                var result = await adminService.DeleteFeedback(deleteFeedback.Id);
                return Ok(new { Message = "Feedback deleted successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("FetchAllServerLogs")]
        public async Task<IActionResult> FetchAllServerLogs()
        {
            try
            {
                var result = await adminService.FetchAllServerLogs();
                return Ok(new { Message = "Server logs fetched successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("DeleteServerLog")]
        public async Task<IActionResult> DeleteServerLog([FromBody] DeleteServerLog deleteServerLog)
        {
            try
            {
                var result = await adminService.DeleteServerLog(deleteServerLog.Id);
                return Ok(new { Message = "Server log deleted successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
