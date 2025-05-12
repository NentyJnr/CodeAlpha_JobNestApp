using JobNest.Abstractions.Services;
using JobNest.Controllers;
using JobNest.Dtos.Profiles.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("get-profile")]
        public async Task<IActionResult> GetProfile()
        {
            var response = await _profileService.GetProfileAsync();

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
           var response =  await _profileService.UpdateProfileAsync(request);

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


