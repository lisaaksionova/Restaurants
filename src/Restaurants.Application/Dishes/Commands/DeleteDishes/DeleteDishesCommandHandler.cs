using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesCommandHandler(ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IRestaurantAuthorizationService authorizationService) : IRequestHandler<DeleteDishesCommand>
{
    public async Task Handle(DeleteDishesCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting dishes from restaurant with id: {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if(restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        if (!authorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();
        
        await dishesRepository.DeleteAsync(restaurant.Dishes);
    }
}