using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantsService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await service.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var restaurant = await service.GetRestaurantById(id);
        if(restaurant is null)
            return NotFound();
        
        return Ok(restaurant);
    }
}