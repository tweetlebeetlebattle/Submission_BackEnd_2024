using Backend.DTO.ModifiedDataDTO.Weightlifter;
using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.DTO.RequestResponseDTOs.Weightlifter;

namespace Backend.Services
{
    public interface IWeightlifterService
    {
        Task<string> CreateNewBlogAsync(string text, IFormFile image, string userId);
        Task<string> CreateNewCommentAsync(string blogId, string text, IFormFile image, string userId);
        Task<List<BlogWithComments>> FetchAllApprovedCommentsAsync(int skip, int blogsPerPage);
        Task<int> FetchNumberOfBlogs();
        Task<int> FetchNumberOfApprovedUserWeightlifterBlogs(string query);
        Task<AllUniversalLogsAndTraining> FetchAllUserTrainingAndUniversalLogs(string userId);
        Task<AllUniversalLogsAndTraining> FetchPublicUserData(string username);
        Task<List<string>> FetchAllTrainingUnits();
        Task<List<string>> FetchAllTrainingTitles();
        Task CreateNewUniversalReading(string userId, CreateUniversalReading createUniversalReading);
        Task CreateNewTraining(string userId, CreateNewTraining createNewTraining);
        Task UpdateUniversalReadingPublicity(ChangeUniversalReadingTrainingPublicity changeForm);
        Task UpdateTrainingLogPublicity(ChangeUniversalReadingTrainingPublicity changeForm);
        Task<List<BlogWithComments>> FetchApprovedUserWeightlifterBlogs(int skip, int pageNumber, string username);
    }
}
