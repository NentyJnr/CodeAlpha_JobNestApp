using JobNest.Entities;
using JobNest.Enums;

namespace JobNest.Entities
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Jobs Jobs { get; set; } = null!;

        public string ProfileId { get; set; }
        public Profile Profile { get; set; } = null!; 

        public string CoverLetterUrl { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
    }
}



