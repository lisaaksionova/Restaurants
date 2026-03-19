using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    
    public CreateRestaurantCommandValidator()
    {
        RuleFor(restaurant => restaurant.Name).NotNull().NotEmpty()
            .Length(3, 100);

        RuleFor(restaurant => restaurant.Category)
            /*.Custom((category, context) =>
            {
                var isValidCategory = validCategories.Contains(category);
                if (!isValidCategory)
                    context.AddFailure("Category", "Invalid category.");
            });*/
            .Must(validCategories.Contains)
            .WithMessage("Provide a valid category name.");
        
        RuleFor(restaurant => restaurant.ContactEmail)
            .EmailAddress()
            .WithMessage("Provide valid email address.");
        RuleFor(restaurant => restaurant.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Provide valid postal code.");
    }
}