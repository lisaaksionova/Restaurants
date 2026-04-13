using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks.Dataflow;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(UserContext))]
public class UserContextTest
{

    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // arrange
        var birthDate = new DateOnly(1999, 12, 31);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "German"),
            new("BirthDate",birthDate.ToString("yyyy-MM-dd")),
        };
        
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext()
            {
                User = user
            });
        
        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        // act
        var currentUser = userContext.GetCurrentUser();
        
        // asset
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.BirthDate.Should().Be(birthDate);
    }
    
    [Fact]
    public void GetCurrentUser_WithUseContextNotPresent_ThrowsInvalidOperationException()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns((HttpContext)null);
        
        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        // act
        Action action = () => userContext.GetCurrentUser();
        
        // asset
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context doesn't exist");
    }
}