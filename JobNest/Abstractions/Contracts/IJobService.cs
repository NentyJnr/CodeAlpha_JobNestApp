using JobNest.Commons.Responses;
using JobNest.Dtos.Jobs.Requests;
using JobNest.Dtos.Jobs.Responses;

namespace JobNest.Abstractions.Services
{
    public interface IJobService
    {
        Task<ApiResponse<Guid>> CreateJobAsync(CreateJobRequest request, string employerId);
        Task<ApiResponse<Guid>> UpdateJobAsync(Guid jobId, UpdateJobRequest request);
        Task<ApiResponse<JobResponse?>> GetJobByIdAsync(Guid jobId);
        Task<ApiResponse<IEnumerable<JobResponse>>> GetAllJobsAsync(string? keyword = null, string? location = null, string? category = null);
        Task<ApiResponse<IEnumerable<JobResponse>>> GetEmployerJobsAsync(string employerId);
        Task<ApiResponse<bool>> DeleteJobAsync(Guid jobId);
    }
}
