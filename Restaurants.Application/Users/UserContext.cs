using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Users;

public class UserContext(IHttpContextAccessor accessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = accessor?.HttpContext?.User;
        if(user == null)
            throw new InvalidOperationException("User context doesn't exist");
        if (user.Identity is not { IsAuthenticated: true })
            return null;
        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
        var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;
        var birthDateString = user.FindFirst(c => c.Type == "BirthDate")?.Value;
        var birthDate = birthDateString == null ? (DateOnly?)null : DateOnly.Parse(birthDateString);
        
        return new CurrentUser(userId, email, roles, nationality, birthDate);
    }
}