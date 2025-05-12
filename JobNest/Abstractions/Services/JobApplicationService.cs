using JobNest.Abstractions.Contracts;
using JobNest.Abstractions.Services;
using JobNest.Commons.Responses;
using JobNest.Data;
using JobNest.Dtos.JobApplications.Requests;
using JobNest.Dtos.JobApplications.Responses;
using JobNest.Entities;
using JobNest.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobNest.Abstractions.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public JobApplicationService(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        
        public async Task<ApiResponse<Guid>> ApplyAsync(ApplyJobRequest request)
        {
            var response = new ApiResponse<Guid>();
            try
            {
                // Getting current user info from token
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    response.Status = false;
                    response.Message = "Token did not contain a valid user ID.";
                    return response;
                }

                var user = await _context.ApplicationUsers
                    .Include(u => u.Profile)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    response.Status = false;
                    response.Message = $"User with ID {userId} not found.";
                    return response;
                }

                if (user.Profile == null)
                {
                    response.Status = false;
                    response.Message = $"User with ID {userId} does not have a profile.";
                    return response;
                }

                var existingApplication = await _context.JobApplications
                    .FirstOrDefaultAsync(a => a.ProfileId == user.Profile.Id && a.JobId == request.JobId);

                if (existingApplication != null)
                {
                    response.Status = false;
                    response.Message = "You have already applied for this job.";
                    return response;
                }
                var application = new JobApplication
                {
                    Id = Guid.NewGuid(),
                    ProfileId = user.Profile.Id,
                    JobId = request.JobId,
                    CoverLetterUrl = request.CoverLetterUrl,
                    ResumeUrl = user.Profile.ResumeUrl,
                    AppliedOn = DateTime.UtcNow,
                    Status = ApplicationStatus.Pending
                };

                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Message = "Job application submitted successfully.";
                response.Data = application.Id;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ApiResponse<IEnumerable<GetApplicationResponse>>> GetUserApplicationsAsync()
        {
            var response = new ApiResponse<IEnumerable<GetApplicationResponse>>();

            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    response.Status = false;
                    response.Message = "Token did not contain a valid user ID.";
                    return response;
                }
                var applications = await _context.JobApplications
                    .Include(a => a.Jobs)
                    .Include(a => a.Profile)
                    .Where(a => a.Profile.UserId == userId)
                    .Select(a => new GetApplicationResponse
                    {
                        Id = a.Id,
                        JobId = a.JobId,
                        JobTitle = a.Jobs.Title,
                        Status = a.Status.ToString(),
                        AppliedOn = a.AppliedOn,
                        ResumeUrl = a.ResumeUrl,
                        CoverLetterUrl = a.CoverLetterUrl
                    })
                    .ToListAsync();

                response.Status = true;
                response.Message = applications.Any()
                    ? "User applications retrieved successfully."
                    : "No applications found for this user.";
                response.Data = applications;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while fetching user applications." };
            }
            return response;
        }

        public async Task<ApiResponse<IEnumerable<ApplicationResponse>>> GetJobApplicationsAsync(Guid jobId)
        {
            var response = new ApiResponse<IEnumerable<ApplicationResponse>>();

            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    response.Status = false;
                    response.Message = "Token did not contain a valid user ID.";
                    return response;
                }
                var applications = await _context.JobApplications
                    .Include(a => a.Jobs)
                    .Include(a => a.Profile)
                        .ThenInclude(p => p.User)
                    .Where(a => a.JobId == jobId)
                    .ToListAsync();

                if (!applications.Any())
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "No applications found for this job." };
                    return response;
                }

                var result = applications.Select(a => new ApplicationResponse
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    JobTitle = a.Jobs.Title,
                    UserId = a.Profile.UserId,
                    UserName = a.Profile.User.FirstName,
                    ResumeUrl = a.ResumeUrl,
                    CoverLetterUrl = a.CoverLetterUrl,
                    Status = a.Status.ToString(),
                    AppliedOn = a.AppliedOn
                }).ToList();

                response.Status = true;
                response.Message = "Job applications retrieved successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while retrieving job applications." };
            }

            return response;
        }

        public async Task<ApiResponse<bool>> UpdateApplicationStatusAsync(Guid applicationId, ApplicationStatus newStatus)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var application = await _context.JobApplications.FindAsync(applicationId);
                if (application == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "Application not found." };
                    return response;
                }

                application.Status = newStatus;
                application.AppliedOn = DateTime.UtcNow; 

                _context.JobApplications.Update(application);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Message = "Application status updated successfully.";
                response.Data = true;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while updating the application status." };
            }
            return response;
        }
    }
}
