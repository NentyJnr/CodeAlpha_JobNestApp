using JobNest.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace JobNest.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }

        public ICollection<Jobs>? PostedJobs { get; set; }
        public ICollection<JobApplication>? Applications { get; set; }
        public virtual Profile Profile { get; set; }

    }
}



