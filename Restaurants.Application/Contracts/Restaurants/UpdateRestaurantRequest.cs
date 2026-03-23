namespace Restaurants.Application.Contracts.Restaurants;

public class UpdateRestaurantRequest
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool HasDelivery { get; set; }
}