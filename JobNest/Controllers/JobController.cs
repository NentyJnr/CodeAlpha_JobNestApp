using JobNest.Abstractions.Services;
using JobNest.Dtos.Jobs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }
        [HttpPost("create-job")]
        public async Task<IActionResult> Create([FromBody] CreateJobRequest request, string employerId)
        {
            var response = await _jobService.CreateJobAsync(request, employerId);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-job")]
        public async Task<IActionResult> GetJobById(Guid jobId)
        {
            var response = await _jobService.GetJobByIdAsync(jobId);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-all-jobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            var response = await _jobService.GetAllJobsAsync();

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("update-job")]
        public async Task<IActionResult> Update([FromQuery] Guid jobId, [FromBody] UpdateJobRequest request)
        {
            var response = await _jobService.UpdateJobAsync(jobId, request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("delete-job/{id}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var response = await _jobService.DeleteJobAsync(id);

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
