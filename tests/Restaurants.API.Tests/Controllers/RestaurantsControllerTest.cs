using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Controllers;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

[TestSubject(typeof(RestaurantsController))]
public class RestaurantsControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepository = new();

    public RestaurantsControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepository.Object));
        }));
    }

    [Fact]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        
        // act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        
        // act
        var result = await client.GetAsync("/api/restaurants");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetById_ForNonExistingId_Returns404NotFound()
    {
        // arrange
        var id = 1123;

        _restaurantsRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Restaurant)null);

        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // arrange
        var id = 99;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test"
        };

        _restaurantsRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var responseContent = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.Id.Should().Be(id);
        responseContent.Name.Should().Be(restaurant.Name);
        responseContent.Description.Should().Be(restaurant.Description);
    }
}