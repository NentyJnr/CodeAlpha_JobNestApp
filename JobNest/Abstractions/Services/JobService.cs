using Azure;
using Azure.Core;
using JobNest.Abstractions.Contracts;
using JobNest.Abstractions.Services;
using JobNest.Commons.Responses;
using JobNest.Data;
using JobNest.Dtos.Jobs.Requests;
using JobNest.Dtos.Jobs.Responses;
using JobNest.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobNest.Abstractions.Services
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public JobService(ApplicationDbContext context, ITokenService tokenService = null)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<Guid>> CreateJobAsync(CreateJobRequest request, string employerId)
        {
            var response = new ApiResponse<Guid>();
            var currentUser = _tokenService.GetToken();
            var userId = currentUser?.UserId;

            var newjob = new Jobs
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                Category = request.Category,
                EmployerId = employerId,
                PostedDate = DateTime.UtcNow,
                IsActive = true,       
                IsDeactivated = false
            };

            _context.Jobs.Add(newjob);
            await _context.SaveChangesAsync();

            return new ApiResponse<Guid>
            {
                Status = true,
                Data = newjob.Id,
                Message = "Job created successfully."
            };
        }

        public async Task<ApiResponse<Guid>> UpdateJobAsync(Guid jobId, UpdateJobRequest request)
        {
            var response = new ApiResponse<Guid>();
            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;
                var exjob = await _context.Jobs.FindAsync(jobId);
                if (exjob is null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "Job not found." };
                    return response;
                }

                exjob.Title = request.Title;
                exjob.Description = request.Description;
                exjob.Location = request.Location;
                exjob.Category = request.Category;
                exjob.DateModified = DateTime.UtcNow;

                _context.Jobs.Update(exjob);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Message = "Job updated successfully.";
                response.Data = exjob.Id;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while updating the job." };
            }
            return response;
        }

        public async Task<ApiResponse<JobResponse>> GetJobByIdAsync(Guid jobId)
        {
            var response = new ApiResponse<JobResponse>();
            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;
                var job = await _context.Jobs
                    .Include(j => j.Employer)
                    .FirstOrDefaultAsync(j => j.Id == jobId);

                if (job is null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "Job not found." };
                    return response;
                }

                var jobResponse = new JobResponse
                {
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    Location = job.Location,
                    Category = job.Category,
                    PostedDate = job.PostedDate,
                    EmployerName = job.Employer.FirstName
                };

                response.Status = true;
                response.Message = "Job retrieved successfully.";
                response.Data = jobResponse;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while retrieving the job." };
            }
            return response;
        }

        public async Task<ApiResponse<IEnumerable<JobResponse>>> GetAllJobsAsync(string? keyword, string? location, string? category)
        {
            var response = new ApiResponse<IEnumerable<JobResponse>>();
            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;
                var query = _context.Jobs.Include(j => j.Employer).AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(j => j.Title.Contains(keyword));

                if (!string.IsNullOrEmpty(location))
                    query = query.Where(j => j.Location == location);

                if (!string.IsNullOrEmpty(category))
                    query = query.Where(j => j.Category == category);

                var jobs = await query.OrderByDescending(j => j.PostedDate).ToListAsync();

                var jobResponses = jobs.Select(j => new JobResponse
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Category = j.Category,
                    PostedDate = j.PostedDate,
                    EmployerName = j.Employer.FirstName
                }).ToList();

                response.Status = true;
                response.Message = "Jobs retrieved successfully.";
                response.Data = jobResponses;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while retrieving jobs." };
            }
            return response;
        }

        public async Task<ApiResponse<IEnumerable<JobResponse>>> GetEmployerJobsAsync(string employerId)
        {
            var response = new ApiResponse<IEnumerable<JobResponse>>();
            try
            {
                var currentUser = _tokenService.GetToken();
                var userId = currentUser?.UserId;
                var jobs = await _context.Jobs
                    .Where(j => j.EmployerId == employerId)
                    .Include(j => j.Employer)
                    .ToListAsync();

                var jobResponses = jobs.Select(j => new JobResponse
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Category = j.Category,
                    PostedDate = j.PostedDate,
                    EmployerName = j.Employer?.FirstName ?? string.Empty
                }).ToList();

                response.Status = true;
                response.Message = "Employer jobs retrieved successfully.";
                response.Data = jobResponses;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while retrieving the employer's jobs." };
            }
            return response;
        }

        public async Task<ApiResponse<bool>> DeleteJobAsync(Guid jobId)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var job = await _context.Jobs.FindAsync(jobId);
                if (job is null)
                {
                    response.Status = false;
                    response.Message = "Job not found.";
                    response.Errors = new List<string> { "Job not found." };
                    return response;
                }

                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Message = "Job deleted successfully.";
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while deleting the job." };
            }
            return response;
        }
    }
}



