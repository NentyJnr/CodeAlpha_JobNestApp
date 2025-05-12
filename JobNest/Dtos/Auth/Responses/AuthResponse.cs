using JobNest.Enums;

namespace JobNest.Dtos.Auth.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType UserType { get; set; }
    }
}
