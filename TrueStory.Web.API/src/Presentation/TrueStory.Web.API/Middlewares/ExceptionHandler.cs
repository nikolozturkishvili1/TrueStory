using FluentValidation;
using Newtonsoft.Json;
using TrueStory.Common.Exceptions;

namespace TrueStory.API.Middlewares;

/// <summary>
/// Middleware to handle exceptions and return standardized JSON responses.
/// </summary>
/// <param name="_next">The next middleware in the pipeline.</param>
public class ExceptionHandler(RequestDelegate _next)
{
    /// <summary>
    /// Processes the HTTP request and handles any exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions and writes an appropriate JSON response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception that was thrown.</param>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            AlreadyExistsException => StatusCodes.Status409Conflict,
            BadRequestException => StatusCodes.Status400BadRequest,
            Common.Exceptions.ArgumentException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var result = JsonConvert.SerializeObject(new
        {
            error = exception.Message
        });

        return context.Response.WriteAsync(result);
    }
}