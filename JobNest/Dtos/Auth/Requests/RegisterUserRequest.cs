using JobNest.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobNest.Dtos.Auth.Requests
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty.ToString();
        public UserType UserType { get; set; }
    }
}

