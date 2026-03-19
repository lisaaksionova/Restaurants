using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(restaurant => restaurant.Name).NotNull().NotEmpty()
            .Length(3, 100);
        RuleFor(restaurant => restaurant.Description).NotNull().NotEmpty();
    }
}