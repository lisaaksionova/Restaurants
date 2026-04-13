using MediatR;
using Restaurants.Application.Contracts.Restaurants;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommand(int id, UpdateRestaurantRequest  request) : IRequest
{
    public int Id { get; } = id;
    public string Name { get; set; } = request.Name;
    public string Description { get; set; } = request.Description;
    public bool HasDelivery { get; set; } = request.HasDelivery;
}