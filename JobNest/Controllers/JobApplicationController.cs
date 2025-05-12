using JobNest.Abstractions.Services;
using JobNest.Dtos.JobApplications.Requests;
using JobNest.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobAppService;

        public JobApplicationController(IJobApplicationService jobAppService)
        {
            _jobAppService = jobAppService;
        }


        [HttpPost("Apply")]
        public async Task<IActionResult> Application([FromBody] ApplyJobRequest request)
        {
            var response = await _jobAppService.ApplyAsync(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("get-user-applications")]
        public async Task<IActionResult> GetUserJobApplications()
        {
            var response = await _jobAppService.GetUserApplicationsAsync();

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpGet("get-job-Applications")]
        public async Task<IActionResult> GetJobApplications(Guid jobId)
        {
            var response = await _jobAppService.GetJobApplicationsAsync(jobId);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("update-jobApp")]
        public async Task<IActionResult> Create([FromBody] Guid applicationId, ApplicationStatus newStatus)
        {
            var response = await _jobAppService.UpdateApplicationStatusAsync(applicationId, newStatus);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
    }
}

