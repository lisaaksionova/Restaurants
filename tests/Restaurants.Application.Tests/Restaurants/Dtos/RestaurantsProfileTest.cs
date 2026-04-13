using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Restaurants.Application.Contracts.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

[TestSubject(typeof(RestaurantsProfile))]
public class RestaurantsProfileTest
{
    private readonly IMapper _mapper;

    public RestaurantsProfileTest()
    {
        var configuration =
            new MapperConfiguration(cfg => { cfg.AddProfile<RestaurantsProfile>(); }, NullLoggerFactory.Instance);

        _mapper = configuration.CreateMapper();
    }
    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // arrange

        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Restaurant",
            Description = "Restaurant",
            Category = "Restaurant",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "1234567890",
            Address = new Address
            {
                City = "Restaurant",
                Street = "Restaurant",
                PostalCode = "12-345"
            }
        };

        // act

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }
    
    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        
        var command = new CreateRestaurantCommand
        {
            Name = "Restaurant",
            Description = "Restaurant",
            Category = "Restaurant",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "1234567890",
            City = "Restaurant",
            Street = "Restaurant",
            PostalCode = "12-345"
        };

        // act

        var restaurant = _mapper.Map<Restaurant>(command);

        // assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }
    
    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var request = new UpdateRestaurantRequest()
        {
            Name = "Restaurant",
            Description = "Restaurant",
            HasDelivery = true
        };

        var command = new UpdateRestaurantCommand(1, request);

        // act

        var restaurant = _mapper.Map<Restaurant>(command);

        // assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }
}