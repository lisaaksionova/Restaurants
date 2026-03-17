using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddAutoMapper(cfg => { }, typeof(ServiceCollectionExtension).Assembly);
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtension).Assembly)
            .AddFluentValidationAutoValidation();
    }
}