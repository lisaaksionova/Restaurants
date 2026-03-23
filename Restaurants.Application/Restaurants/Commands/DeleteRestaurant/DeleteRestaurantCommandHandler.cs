using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with ID : {RestaurantId}", request.Id);
        
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant == null)
        {
            logger.LogError("Restaurant with ID : {RestaurantId} not found", request.Id);
            return false;
        }
        
        await restaurantsRepository.DeleteAsync(restaurant);
        return true;
    }
}