namespace Restaurants.Application.Users;

public record CurrentUser(string Id, string Email, IEnumerable<string> Roles, string? Nationality, DateOnly? BirthDate)
{
    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }
}