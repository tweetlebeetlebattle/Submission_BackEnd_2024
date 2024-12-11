using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.DTO.RequestResponseDTOs.Shared;

namespace Backend.Repositories
{
    public interface IDiverRepository
    {
        Task PostUserFeedback(
            string userId,
            int locationId,
            float? waveRead = null,
            int? waveUnitId = null,
            float? tempRead = null,
            int? tempUnitId = null,
            float? windSpeedIndex = null,
            int? windSpeedUnitId = null,
            string? pictureUrl = null,
            string? textUrl = null);

        Task<HistoricSeaDataByLocationDate> FetchIndexSeaDataByPeriod(int period);
        Task<List<BlogWithComments>> FetchAllApprovedBlogDataAsync(int skip, int blogsPerPage);
        Task CreateNewBlog(string userId, string textUrl, string pictureUrl);
        Task CreateNewCommentAsync(string parentBlogId, string userId, string textUrl, string pictureUrl);
        Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationHTML(string location);
        Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationGif(string location);
        Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationStorm(string location);
        Task<int> FetchNumberOfBlogs();
        Task<int> FetchNumberOfApprovedUserDiverBlogs(string query);
        Task<List<BlogWithComments>> FetchApprovedUserDiverBlogs(int skip, int blogsPerPage, string username);
    }
}
