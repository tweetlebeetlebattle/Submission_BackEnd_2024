using Backend.DTO;
using Backend.DTO.RequestResponseDTOs.Admin;
using Backend.Repositories;

namespace Backend.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepository;

        public AdminService(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<UnapprovedBlogCommentData> FetchAllUnapprovedBlogCommentsAsync()
        {
            return await _adminRepository.FetchAllUnapprovedBlogComments();
        }

        public async Task<string> ApproveOrRejectBlogCommentAsync(string id, string status)
        {
            await _adminRepository.ApproveOrDeleteBlogComment(id, status);
            return $"Successfully {status} entity with ID: {id}";
        }
        public async Task<FeedbacksToDisplay> FetchAllFeedbacks()
        {
            return await _adminRepository.FetchAllFeedbacks();
        }

        public async Task<string> DeleteFeedback(string id)
        {
            await _adminRepository.DeleteFeedback(id);
            return $"Successfully deleted entity with ID: {id}";
        }

    }
}
