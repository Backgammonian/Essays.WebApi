using System.Security.Claims;

namespace Essays.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        ClaimsPrincipal? GetCurrentUser();
    }
}
