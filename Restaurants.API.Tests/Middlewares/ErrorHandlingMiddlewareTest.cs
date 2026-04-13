using System;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

[TestSubject(typeof(ErrorHandlingMiddleware))]
public class ErrorHandlingMiddlewareTest
{

    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNetDelegate()
    {
        // arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();
        
        // act
        
        await middleware.InvokeAsync(context, nextDelegateMock.Object);
        
        // assert
        
        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusTo404()
    {
        // arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");
        
        // act
        
        await middleware.InvokeAsync(context, _ => throw notFoundException);
        
        // assert
        
        context.Response.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenForbiddenExceptionThrown_ShouldSetStatusTo403()
    {
        // arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var forbiddenException = new ForbidException();
        
        // act
        
        await middleware.InvokeAsync(context, _ => throw forbiddenException);
        
        // assert
        
        context.Response.StatusCode.Should().Be(403);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenExceptionThrown_ShouldSetStatusTo500()
    {
        // arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception();
        
        // act
        
        await middleware.InvokeAsync(context, _ => throw exception);
        
        // assert
        
        context.Response.StatusCode.Should().Be(500);
    }
}