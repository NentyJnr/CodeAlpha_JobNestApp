using JobNest.Commons.Responses;
using JobNest.Dtos.Auth.Requests;
using JobNest.Dtos.Auth.Responses;

namespace JobNest.Abstractions.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterUserRequest request);
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginUserRequest request);
        Task<ApiResponse<bool>> LogoutAsync();
    }
}

