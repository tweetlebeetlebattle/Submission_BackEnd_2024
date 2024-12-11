using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IJWTService
    {
        string GenerateJwtTokenByEmail(string email);
        string? RetrieveEmailFromToken(string token);
        Task<string> GetUserIdFromJwtAsync(string authorizationHeader);
    }
}
