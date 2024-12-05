using Backend.Data.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class UtilityService
    {
        private readonly UtilsRepository _utilsRepository;
        private readonly S3BucketAWSService _bucketAWSService;
        public UtilityService(UtilsRepository utilsRepository, S3BucketAWSService bucketAWSService)
        {
            _utilsRepository = utilsRepository;
            _bucketAWSService = bucketAWSService;
        }
        public async Task<List<string>> FetchSearchSuggestions(string searchQuery)
        {
            return await _utilsRepository.FetchSearchSuggestions(searchQuery);
        }
        public async Task<int> GetLocationIdByNameAsync(string locationName)
        {
            return await _utilsRepository.GetLocationIdByNameAsync(locationName);
        }

        public async Task<int> GetUnitIdByUnitNameAsync(string unitName)
        {
            return await _utilsRepository.GetUnitIdByUnitNameAsync(unitName);
        }

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            return await _utilsRepository.GetUserIdByEmailAsync(email);
        }
        public async Task<List<string>> GetAllUnitsAsync()
        {
            return await _utilsRepository.GetAllUnitsAsAListAsync();
        }

        public async Task<List<string>> GetAllLocationsAsync()
        {
            return await _utilsRepository.GetAllLocationsAsAListAsync();
        }
        public async Task<int> FetchTrainingUnitIdByName(string name)
        {
            return await _utilsRepository.FetchTrainingUnitIdByName(name);
        }
        public DateTime ConvertStringToDateTime(string dateTimeString)
        {
            if (DateTime.TryParse(dateTimeString, out var result))
            {
                return result;
            }
            else
            {
                throw new FormatException("Invalid date-time format. Please provide a valid date-time string.");
            }
        }
        public async Task<string> CreateNewMediaReturnId(string userId, IFormFile? image, string? text)
        {
            string? imageUrl = null;
            string? textUrl = null;

            if (image != null)
            {
                using var imageStream = image.OpenReadStream();
                string imageKey = $"images/{Guid.NewGuid()}_{image.FileName}";
                imageUrl = await _bucketAWSService.UploadFileAsync("your-bucket-name", imageKey, imageStream);
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                string textKey = $"texts/{Guid.NewGuid()}_text.txt";
                textUrl = await _bucketAWSService.UploadTextAsync("your-bucket-name", textKey, text);
            }

            return await _utilsRepository.CreateNewMediaReturnId(userId, imageUrl, textUrl);
        }

    }
}
