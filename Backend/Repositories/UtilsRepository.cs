using Backend.Data;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UtilsRepository : IUtilsRepository
    {
        private readonly ApplicationDbContext context;

        public UtilsRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<int> GetLocationIdByNameAsync(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
                throw new ArgumentException("Location name cannot be null or empty.", nameof(locationName));

            var location = await context.Set<Locations>()
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Name == locationName);

            if (location == null)
                throw new KeyNotFoundException($"Location '{locationName}' not found.");

            return location.Id;
        }

        public async Task<int> GetUnitIdByUnitNameAsync(string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
                throw new ArgumentException("Unit name cannot be null or empty.", nameof(unitName));

            var unit = await context.Set<Units>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UnitName == unitName);

            if (unit == null)
                throw new KeyNotFoundException($"Unit '{unitName}' not found.");

            return unit.UnitId;
        }

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            var user = await context.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return user?.Id;
        }
        public async Task<List<string>> GetAllUnitsAsAListAsync()
        {
            return await context.Set<Units>()
                .AsNoTracking()
                .Select(unit => unit.UnitName)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllLocationsAsAListAsync()
        {
            return await context.Set<Locations>()
                .AsNoTracking()
                .Select(location => location.Name)
                .ToListAsync();
        }

        public async Task<int> FetchTrainingUnitIdByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Unit name cannot be null or empty.", nameof(name));

            var unit = await context.Set<TrainingUnits>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UnitName.ToLower() == name.ToLower());

            if (unit == null)
            {
                unit = new TrainingUnits()
                {
                    UnitName = name
                };
                await context.TrainingUnits.AddAsync(unit);
                await context.SaveChangesAsync();
            }

            return unit.Id;
        }
        public async Task<string> CreateNewMediaReturnId(string userId, string? imageUrl, string? pictureUrl)
        {
            var newMedia = new Media
            {
                MediaId = Guid.NewGuid().ToString(), 
                TextUrl = imageUrl,
                PictureUrl = pictureUrl,
                ApplicationUserId = userId
            };

            await context.Media.AddAsync(newMedia);

            await context.SaveChangesAsync();

            return newMedia.MediaId;
        }
        public async Task<List<string>> FetchSearchSuggestions(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<string>();
            }

            string lowerSearchQuery = searchQuery.ToLower();

            return await context.ApplicationUsers
                .Where(user => user.UserName.ToLower().Contains(lowerSearchQuery)) 
                .Select(user => user.UserName)
                .ToListAsync();
        }

    }
}
