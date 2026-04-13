using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Contracts.Restaurants;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTest
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _logger;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRestaurantsRepository> _repository;
    private readonly Mock<IRestaurantAuthorizationService> _service;
    
    private readonly UpdateRestaurantCommandHandler _handler;
    
    public UpdateRestaurantCommandHandlerTest()
    {
        _logger = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapper = new Mock<IMapper>();
        _repository = new Mock<IRestaurantsRepository>();
        _service = new Mock<IRestaurantAuthorizationService>();
        
        _handler = new UpdateRestaurantCommandHandler(_logger.Object,
            _mapper.Object,
            _repository.Object,
            _service.Object);
    }

    [Fact]
    public async Task Handle_ForValidCommand_ChangesAreSaved()
    {
        // arrange
        var restaurantId = 1;
        var request = new UpdateRestaurantRequest
        {
            Name = "test",
            Description = "test",
            HasDelivery = true
        };
        var command = new UpdateRestaurantCommand(restaurantId, request);

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "example",
            Description = "example"
        };
        
        _mapper.Setup(m => m.Map<Restaurant>(command)).Returns(new Restaurant());
        _repository.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _service.Setup(s => s.Authorize(restaurant, ResourceOperation.Update)).Returns(true);
        
        // act
        await _handler.Handle(command, CancellationToken.None);

        // assert
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapper.Verify(m => m.Map(command, restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ThrowsException()
    {
        // arrange
        var restaurantId = 1;
        var request = new UpdateRestaurantRequest();
        var command = new UpdateRestaurantCommand(restaurantId, request);
        
        _repository.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant)null);
        
        // act
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        
        // assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} not found");
    }
    
    [Fact]
    public async Task Handle_WithNonAuthorizedUser_ThrowsException()
    {
        // arrange
        var restaurantId = 1;
        var request = new UpdateRestaurantRequest();
        var command = new UpdateRestaurantCommand(restaurantId, request);
        var restaurant = new Restaurant{ Id =  restaurantId };
        
        _repository.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);
        _mapper.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        _service.Setup(s => s.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

        
        // act
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        
        // assert
        await act.Should().ThrowAsync<ForbidException>();
    }
}