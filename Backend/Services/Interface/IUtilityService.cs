namespace Backend.Services
{
    public interface IUtilityService
    {
        Task<List<string>> FetchSearchSuggestions(string searchQuery);
        Task<int> GetLocationIdByNameAsync(string locationName);
        Task<int> GetUnitIdByUnitNameAsync(string unitName);
        Task<string?> GetUserIdByEmailAsync(string email);
        Task<List<string>> GetAllUnitsAsync();
        Task<List<string>> GetAllLocationsAsync();
        Task<int> FetchTrainingUnitIdByName(string name);
        DateTime ConvertStringToDateTime(string dateTimeString);
        Task<string> CreateNewMediaReturnId(string userId, IFormFile? image, string? text);
    }
}
