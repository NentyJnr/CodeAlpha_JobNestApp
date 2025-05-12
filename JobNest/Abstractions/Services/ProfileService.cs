using JobNest.Abstractions.Contracts;
using JobNest.Abstractions.Services;
using JobNest.Commons.Responses;
using JobNest.Data;
using JobNest.Dtos.Jobs.Responses;
using JobNest.Dtos.Profiles.Requests;
using JobNest.Dtos.Profiles.Responses;
using JobNest.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace JobNest.Abstractions.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public ProfileService(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<ProfileResponse>> GetProfileAsync()
        {
            var response = new ApiResponse<ProfileResponse>();

            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;

                var pro = await _context.Profiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);
                if (pro is null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "Job not found." };
                    return response;
                }

                var profileResponse = new ProfileResponse
                {
                    Id = pro.Id,
                    Bio = pro.Bio,
                    Skills = pro.Skills,
                    ResumeUrl = pro.ResumeUrl,
                    LinkedInUrl = pro.LinkedInUrl,
                    PortfolioUrl = pro.PortfolioUrl,
                };

                response.Status = true;
                response.Message = "Job retrieved successfully.";
                response.Data = profileResponse;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while retrieving the job." };
            }
            return response;
        }

        public async Task<ApiResponse<ProfileResponse>> UpdateProfileAsync(UpdateProfileRequest request)
        {
            var response = new ApiResponse<ProfileResponse>();
            var currentUser = _tokenService.GetToken();
            var userId = currentUser?.UserId;

            var user = await _context.ApplicationUsers
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                response.Status = false;
                response.Message = "User not found.";
                return response;
            }

            if (user.Profile == null)
            {
                user.Profile = new Profile
                {
                    Id = Guid.NewGuid().ToString(), 
                    UserId = userId
                };
                _context.Profiles.Add(user.Profile); 
            }
            user.Profile.Bio = request.Bio;
            user.Profile.Skills = request.Skills;
            user.Profile.ResumeUrl = request.ResumeUrl;
            user.Profile.LinkedInUrl = request.LinkedInUrl;
            user.Profile.PortfolioUrl = request.PortfolioUrl;

            await _context.SaveChangesAsync();

            response.Status = true;
            response.Message = "Profile updated successfully.";
            response.Data = new ProfileResponse
            {
                Id = user.Profile.Id,
                Bio = user.Profile.Bio,
                Skills = user.Profile.Skills,
                ResumeUrl = user.Profile.ResumeUrl,
                LinkedInUrl = user.Profile.LinkedInUrl,
                PortfolioUrl = user.Profile.PortfolioUrl
            };
            return response;
        }
    }
}






