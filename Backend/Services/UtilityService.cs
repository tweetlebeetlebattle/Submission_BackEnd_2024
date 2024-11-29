using Backend.Data.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class UtilityService
    {
        private readonly UtilsRepository _utilsRepository;

        public UtilityService(UtilsRepository utilsRepository)
        {
            _utilsRepository = utilsRepository;
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
    }
}
