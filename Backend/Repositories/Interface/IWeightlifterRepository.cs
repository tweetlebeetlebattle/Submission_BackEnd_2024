using Backend.DTO.ModifiedDataDTO.Weightlifter;
using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.DTO.RequestResponseDTOs.Weightlifter;

namespace Backend.Repositories
{
    public interface IWeightlifterRepository
    {
        Task<List<BlogWithComments>> FetchAllApprovedBlogDataAsync(int skip, int blogsPerPage);
        Task<int> FetchNumberOfBlogs();
        Task CreateNewBlog(string userId, string textUrl, string pictureUrl);
        Task CreateNewCommentAsync(string parentBlogId, string userId, string textUrl, string pictureUrl);
        Task<int> GetTrainingUnitIdByUnitNameAsync(string unitName);
        Task<AllUniversalLogsAndTraining> FetchAllUserTrainingAndUniversalReading(string userId);
        Task<AllUniversalLogsAndTraining> FetchPublicUserData(string username);
        Task<List<string>> FetchAllTrainingUnits();
        Task<List<string>> FetchAllTrainingTitles();
        Task CreateNewUniversalReading(string userId, string name, double measurement, int trainingUnitId, DateTime dateTime);
        Task CreateNewTraining(string userId, CreateNewTrainingModified modifiedDTO);
        Task UpdateUniversalReadingPublicity(string title, bool isPublic);
        Task UpdateTrainingLogPublicity(string title, bool isPublic);
        Task<int> FetchNumberOfApprovedUserWeightlifterBlogs(string query);
        Task<List<BlogWithComments>> FetchApprovedUserWeightlifterBlogs(int skip, int blogsPerPage, string username);
    }
}
