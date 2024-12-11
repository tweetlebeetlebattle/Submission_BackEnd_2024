using Backend.DTO.RequestResponseDTOs.Admin;

namespace Backend.Repositories
{
    public interface IAdminRepository
    {
        Task<UnapprovedBlogCommentData> FetchAllUnapprovedBlogComments();
        Task ApproveOrDeleteBlogComment(string id, string status);
        Task<FeedbacksToDisplay> FetchAllFeedbacks();
        Task DeleteFeedback(string id);
        Task<ServerLogs> FetchAllServerLogs();
        Task DeleteServerLog(string id);
    }
}
