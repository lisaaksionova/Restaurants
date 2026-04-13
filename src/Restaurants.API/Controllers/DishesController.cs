using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDishes;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetAllForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishForRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, dishId }, null);    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }
    
    [HttpGet("{dishId:int}")]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = await mediator.Send(new GetDishForRestaurantQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDishes([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteDishesCommand(restaurantId));
        return NoContent();
    }
}