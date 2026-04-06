using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.RestaurantsCount;

public class RestaurantsCountRequirementHandler(ILogger<RestaurantsCountRequirementHandler> logger,
    IUserContext userContext,
    IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<RestaurantsCountRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RestaurantsCountRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        var restaurants = await restaurantsRepository.GetAllAsync();

        if (restaurants.Count(r => r.OwnerId == currentUser!.Id) >= requirement.RestaurantsCount)
        {
            logger.LogInformation("Authorization successful");
            context.Succeed(requirement);
        }
        else
            context.Fail();
    }
}