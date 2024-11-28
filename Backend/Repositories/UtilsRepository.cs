using Backend.Data;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class UtilsRepository
    {
        private readonly ApplicationDbContext _context;

        public UtilsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetLocationIdByNameAsync(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
                throw new ArgumentException("Location name cannot be null or empty.", nameof(locationName));

            var location = await _context.Set<Locations>()
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

            var unit = await _context.Set<Units>()
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

            var user = await _context.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return user?.Id;
        }
    }
}
