namespace Backend.DTO.RequestResponseDTOs.Auth
{
    public class AuthenticationSuccessResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
    }
}
