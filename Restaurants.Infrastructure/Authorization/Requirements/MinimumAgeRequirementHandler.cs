using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
    IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement", currentUser.Email, currentUser.BirthDate);

        if (currentUser.BirthDate == null)
        {
            logger.LogWarning("User: {Email}, date of birth null", currentUser.Email);
            context.Fail();
            return Task.CompletedTask;
        }
        
        if (currentUser.BirthDate.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("Authorization successful");
            context.Succeed(requirement);
        }
        
        context.Fail();
        return Task.CompletedTask;
    }
}