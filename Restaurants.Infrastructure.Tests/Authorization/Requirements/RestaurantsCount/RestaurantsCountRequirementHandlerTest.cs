using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Logging;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements.RestaurantsCount;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements.RestaurantsCount;

[TestSubject(typeof(RestaurantsCountRequirementHandler))]
public class RestaurantsCountRequirementHandlerTest
{

    [Fact]
    public async Task HandleRequirementAsyncTest_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // arrange
        
        var loggerMock = new Mock<ILogger<RestaurantsCountRequirementHandler>>();

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = "2",
            },
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);
        
        var requirement = new RestaurantsCountRequirement(2);
        var handler = new RestaurantsCountRequirementHandler(loggerMock.Object, userContextMock.Object, restaurantsRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // act
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }
    
    [Fact]
    public async Task HandleRequirementAsyncTest_UserHasNotCreatedMultipleRestaurants_ShouldSucceed()
    {
        // arrange
        
        var loggerMock = new Mock<ILogger<RestaurantsCountRequirementHandler>>();

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = "2",
            },
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);
        
        var requirement = new RestaurantsCountRequirement(2);
        var handler = new RestaurantsCountRequirementHandler(loggerMock.Object, userContextMock.Object, restaurantsRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // act
        await handler.HandleAsync(context);

        // assert
        context.HasFailed.Should().BeTrue();
    }
}