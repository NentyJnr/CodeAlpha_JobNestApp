namespace JobNest.Dtos.JobApplications.Responses
{
    public class GetApplicationResponse
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;

        public string CoverLetterUrl { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedOn { get; set; }
    }
}
