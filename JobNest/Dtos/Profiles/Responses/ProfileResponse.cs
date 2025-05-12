namespace JobNest.Dtos.Profiles.Responses
{
    public class ProfileResponse
    {
        public string Id { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;
        public string PortfolioUrl { get; set; } = string.Empty;
    }
}
