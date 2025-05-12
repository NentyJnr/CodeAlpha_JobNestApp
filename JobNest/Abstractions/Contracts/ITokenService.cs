using JobNest.Dtos.Auth.Requests;
using JobNest.Entities;

namespace JobNest.Abstractions.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
        CurrentUser GetToken();

    }
}
