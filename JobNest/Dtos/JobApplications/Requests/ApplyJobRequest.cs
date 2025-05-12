namespace JobNest.Dtos.JobApplications.Requests
{
    public class ApplyJobRequest
    {
        public Guid JobId { get; set; }
        public string ResumeUrl { get; set; } = string.Empty;
        public string CoverLetterUrl { get; set; } = string.Empty;
    }
}
