using Backend.Data; 
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UtilityService
    {
        private readonly ApplicationDbContext _context;

        public UtilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            var user = await _context.Users
                .AsNoTracking() 
                .FirstOrDefaultAsync(u => u.Email == email);

            return user?.Id;
        }
    }
}
