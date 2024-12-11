using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.DTO.RequestResponseDTOs.Shared;

namespace Backend.Services
{
    public interface IDiverService
    {
        Task<string> CreateNewBlogAsync(string text, IFormFile image, string userId);
        Task<string> CreateNewCommentAsync(string blogId, string text, IFormFile image, string userId);
        Task<List<BlogWithComments>> FetchAllApprovedCommentsAsync(int skip, int blogsPerPage);
        Task<int> FetchNumberOfBlogs();
        Task<int> FetchNumberOfApprovedUserDiverBlogs(string query);
        Task<List<BlogWithComments>> FetchApprovedUserDiverBlogs(int skip, int pageNumber, string username);
        Task PostUserFeedback(string userId, FeedbackDTO feedbackDto);
        Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationHTML(string location);
        Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationGif(string location);
        Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationStorm(string location);
        Task<double> ReturnDiveIndexOnData(List<HistoricSeaDataByLocationReadings> data);
    }
}
