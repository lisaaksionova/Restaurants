using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

public interface IRestaurantSeeder
{
    Task SeedAsync();
}