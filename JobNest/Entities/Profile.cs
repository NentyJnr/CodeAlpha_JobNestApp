namespace JobNest.Entities
{
    public class Profile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string Bio { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;
        public string PortfolioUrl { get; set; } = string.Empty;
        public ICollection<JobApplication> JobApplications { get; set; }
    }
}


