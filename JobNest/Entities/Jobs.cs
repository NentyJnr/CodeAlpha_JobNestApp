using JobNest.Entities.Shared;
using Microsoft.AspNetCore.Builder;

namespace JobNest.Entities
{
    public class Jobs : AuditableObject
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public string EmployerId { get; set; }
        public ApplicationUser Employer { get; set; } = null!;

        public ICollection<JobApplication>? Applications { get; set; }
    }
}

