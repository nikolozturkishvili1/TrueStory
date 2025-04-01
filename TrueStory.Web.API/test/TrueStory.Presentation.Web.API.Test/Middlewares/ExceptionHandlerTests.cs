using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using FluentValidation;
using TrueStory.API.Middlewares;
using TrueStory.Common.Exceptions;

/// <summary>
/// Unit tests for the <see cref="ExceptionHandler"/> middleware.
/// </summary>
public class ExceptionHandlerTests
{
    private readonly RequestDelegate _next;
    private readonly ExceptionHandler _exceptionHandler;
    private readonly DefaultHttpContext _context;

    public ExceptionHandlerTests()
    {
        _next = Substitute.For<RequestDelegate>();
        _exceptionHandler = new ExceptionHandler(_next);
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldReturn404()
    {
        // Arrange
        _next.Invoke(Arg.Any<HttpContext>()).Throws(new NotFoundException("Resource not found"));

        // Act
        await _exceptionHandler.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        await AssertResponseMessage(_context, "Resource not found");
    }

    [Fact]
    public async Task InvokeAsync_WhenAlreadyExistsExceptionThrown_ShouldReturn409()
    {
        // Arrange
        _next.Invoke(Arg.Any<HttpContext>()).Throws(new AlreadyExistsException("Entity already exists"));

        // Act
        await _exceptionHandler.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        await AssertResponseMessage(_context, "Entity already exists");
    }

    [Fact]
    public async Task InvokeAsync_WhenBadRequestExceptionThrown_ShouldReturn400()
    {
        // Arrange
        _next.Invoke(Arg.Any<HttpContext>()).Throws(new BadRequestException("Invalid request"));

        // Act
        await _exceptionHandler.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        await AssertResponseMessage(_context, "Invalid request");
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationExceptionThrown_ShouldReturn400()
    {
        // Arrange
        _next.Invoke(Arg.Any<HttpContext>()).Throws(new ValidationException("Validation failed"));

        // Act
        await _exceptionHandler.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        await AssertResponseMessage(_context, "Validation failed");
    }

    [Fact]
    public async Task InvokeAsync_WhenUnhandledExceptionThrown_ShouldReturn500()
    {
        // Arrange
        _next.Invoke(Arg.Any<HttpContext>()).Throws(new Exception("Unexpected error"));

        // Act
        await _exceptionHandler.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        await AssertResponseMessage(_context, "Unexpected error");
    }

    private static async Task AssertResponseMessage(HttpContext context, string expectedMessage)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8);
        var responseJson = await reader.ReadToEndAsync();
        var response = JsonSerializer.Deserialize<Dictionary<string, string>>(responseJson);

        response.Should().ContainKey("error");
        response["error"].Should().Be(expectedMessage);
    }
}
