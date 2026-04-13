using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandValidator))]
public class CreateRestaurantCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "12-345",
        };
        
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "te",
            Category = "test",
            ContactEmail = "testest.com",
            PostalCode = "12345",
        };
        
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCommand_ShouldHaveNoErrors(string category)
    {
        // arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { Category = category };
        
        // act
        var result = validator.TestValidate(command);
        
        // assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory]
    [InlineData("10345")]
    [InlineData("103-45")]
    [InlineData("1 345")]
    public void Validator_ForInvalidPostalCode_ShouldHaveErrors(string postalCode)
    {
        // arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PostalCode = postalCode };
        
        // act
        var result = validator.TestValidate(command);
        
        // assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

}