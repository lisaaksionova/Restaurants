using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext context) : IDishesRepository
{
    public async Task<int> CreateAsync(Dish dish)
    {
        context.Dishes.Add(dish);
        await context.SaveChangesAsync();
        return dish.Id;
    }

    public async Task DeleteAsync(IEnumerable<Dish> dishes)
    {
        context.Dishes.RemoveRange(dishes);
        await context.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}