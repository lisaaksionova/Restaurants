using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetAllForRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishForRestaurant;

public class GetDishForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper): IRequestHandler<GetDishForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dish with id {DishId} for restaurant with id: {RestaurantId}", request.DishId, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
        if (dish == null)
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        return mapper.Map<DishDto>(dish);
    }
}