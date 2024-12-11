namespace Backend.Repositories
{
    public interface IUtilsRepository
    {
        Task<int> GetLocationIdByNameAsync(string locationName);
        Task<int> GetUnitIdByUnitNameAsync(string unitName);
        Task<string?> GetUserIdByEmailAsync(string email);
        Task<List<string>> GetAllUnitsAsAListAsync();
        Task<List<string>> GetAllLocationsAsAListAsync();
        Task<int> FetchTrainingUnitIdByName(string name);
        Task<string> CreateNewMediaReturnId(string userId, string? imageUrl, string? pictureUrl);
        Task<List<string>> FetchSearchSuggestions(string searchQuery);
    }
}
