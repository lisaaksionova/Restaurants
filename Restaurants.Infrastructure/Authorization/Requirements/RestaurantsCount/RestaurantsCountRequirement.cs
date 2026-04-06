using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.RestaurantsCount;

public class RestaurantsCountRequirement(int restaurantsCount) : IAuthorizationRequirement
{
    public int RestaurantsCount { get; } = restaurantsCount;
}