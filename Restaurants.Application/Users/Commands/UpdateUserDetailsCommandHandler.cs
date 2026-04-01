using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
    IUserContext context,
    IUserStore<User> store) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = context.GetCurrentUser();
        
        logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id, request);
        
        var dbUser = await store.FindByIdAsync(user!.Id, cancellationToken);
        if (dbUser == null)
            throw new NotFoundException(nameof(User), user!.Id);
        dbUser.BirthDate = request.BirthDate;
        dbUser.Nationality = request.Nationality;
        
        await store.UpdateAsync(dbUser, cancellationToken);
    }
}