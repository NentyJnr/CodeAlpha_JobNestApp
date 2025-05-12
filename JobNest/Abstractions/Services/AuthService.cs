using JobNest.Abstractions.Contracts;
using JobNest.Commons.Responses;
using JobNest.Data;
using JobNest.Dtos.Auth.Requests;
using JobNest.Dtos.Auth.Responses;
using JobNest.Entities;
using JobNest.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobNest.Abstractions.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterUserRequest request)
        {
            var response = new ApiResponse<AuthResponse>();
            var exUser = await _context.ApplicationUsers.FirstOrDefaultAsync(p => p.Email == request.Email);
            if (exUser != null)
            {
                response.Status = false;
                response.Message = "record exist";
                return response;
            }
            var role = UserType.JobSeeker.ToString();

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.Status = false;
                response.Message = "Registration failed";
                return response;
            }

            user.Profile = new Profile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ResumeUrl = "",   
            };

            _context.Profiles.Add(user.Profile);
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            response.Status = true;
            response.Message = "Registration successful";
            response.Data = new AuthResponse
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserType = request.UserType
            };
            return response;
        }


        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginUserRequest request)
        {
            var response = new ApiResponse<AuthResponse>();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.Status = false;
                response.Message = "Invalid user";
                return response;

            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    response.Status = false;
                    response.Message = "User locked out";
                    return response;
                }
                response.Status = false;
                response.Message = "Invalid password";
                return response;
            }

            var token = _tokenService.GenerateToken(user);
            if (token == null || string.IsNullOrEmpty(token))
            {
                response.Status = false;
                response.Message = "invalid token";
            }
            response.Status = true;
            response.Message = "Login successful";
            response.Data = new AuthResponse
            {
                Token = token,
                FirstName = response.Data.FirstName,
                LastName = response.Data.LastName,
                Email = response.Data.Email,
                UserType = response.Data.UserType
            };
            return response;
        }


        public async Task<ApiResponse<bool>> LogoutAsync()
        {
            var response = new ApiResponse<bool>();
            var token = _tokenService.GetToken();
            if (token == null)
            {
                response.Status = false;
                response.Message = "invalid token";
                return response;
            }
            var user = token.UserName;
            if (user is null)
            {
                response.Status = false;
                response.Message = "invalid user";
                return response;
            }

            await _signInManager.SignOutAsync();
            return response;
        }

    }
}




