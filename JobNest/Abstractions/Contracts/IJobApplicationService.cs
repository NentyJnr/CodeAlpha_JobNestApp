using JobNest.Commons.Responses;
using JobNest.Dtos.JobApplications.Requests;
using JobNest.Dtos.JobApplications.Responses;
using JobNest.Enums;

namespace JobNest.Abstractions.Services
{
    public interface IJobApplicationService
    {
        Task<ApiResponse<Guid>> ApplyAsync(ApplyJobRequest request);
        Task<ApiResponse<IEnumerable<GetApplicationResponse>>> GetUserApplicationsAsync();
        Task<ApiResponse<IEnumerable<ApplicationResponse>>> GetJobApplicationsAsync(Guid jobId);
        Task<ApiResponse<bool>> UpdateApplicationStatusAsync(Guid applicationId, ApplicationStatus newStatus);
    }
}

