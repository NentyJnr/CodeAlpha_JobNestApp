using JobNest.Commons.Responses;
using JobNest.Dtos.Profiles.Requests;
using JobNest.Dtos.Profiles.Responses;

namespace JobNest.Abstractions.Services
{
    public interface IProfileService
    {
        Task<ApiResponse<ProfileResponse>> GetProfileAsync();
        Task <ApiResponse<ProfileResponse>> UpdateProfileAsync(UpdateProfileRequest request);
    }
}

