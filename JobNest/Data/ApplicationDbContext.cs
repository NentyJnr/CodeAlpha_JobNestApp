using JobNest.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobNest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<JobApplication> JobApplications { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);

            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Profile)
                .WithMany()
                .HasForeignKey(a => a.ProfileId);

            modelBuilder.Entity<JobApplication>()
                .HasOne(j => j.Profile)
                .WithMany()
                .HasForeignKey(j => j.ProfileId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<JobApplication>()
                .HasOne(j => j.Profile)
                .WithMany(p => p.JobApplications)
                .HasForeignKey(j => j.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(j => j.Jobs)
                .WithMany()
                .HasForeignKey(j => j.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Jobs>()
                .HasOne(j => j.Employer)
                .WithMany(e => e.PostedJobs)
                .HasForeignKey(j => j.EmployerId)
                .IsRequired();
        }
    }
}
