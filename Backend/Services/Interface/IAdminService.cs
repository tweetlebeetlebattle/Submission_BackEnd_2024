using Backend.DTO.RequestResponseDTOs.Admin;

namespace Backend.Services
{
    public interface IAdminService
    {
        Task<UnapprovedBlogCommentData> FetchAllUnapprovedBlogCommentsAsync();
        Task<string> ApproveOrRejectBlogCommentAsync(string id, string status);
        Task<FeedbacksToDisplay> FetchAllFeedbacks();
        Task<string> DeleteFeedback(string id);
        Task<ServerLogs> FetchAllServerLogs();
        Task<string> DeleteServerLog(string id);
    }
}
