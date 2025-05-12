using JobNest.Enums;

namespace JobNest.Dtos.JobApplications.Requests
{
    public class UpdateApplicationStatusRequest
    {
        public ApplicationStatus Status { get; set; }
    }
}
