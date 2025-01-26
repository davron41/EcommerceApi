using Microsoft.AspNetCore.Mvc;
using Twewew.Exceptions;

namespace Twewew.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(ex, context);
        }
    }

    private async Task HandleAsync(Exception exception, HttpContext context)
    {

        var details = GetErrorDetails(exception);

        context.Response.StatusCode = details.Status!.Value;

        await context.Response
            .WriteAsJsonAsync(details);
    }

    private static ProblemDetails GetErrorDetails(Exception exception)
        => exception switch
        {
            EntityNotFoundException => new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "Not Found", Detail = exception.Message },
            UsernameAlreadyExistsException => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "User Name already exists", Detail = exception.Message },
            InvalidLoginRequestException => new ProblemDetails { Status = StatusCodes.Status401Unauthorized, Title = "Invalid username or password", Detail = "Verify that the account with given username exists and password matches" },
            _ => new ProblemDetails { Status = StatusCodes.Status500InternalServerError, Title = "Internal Server Error", Detail = exception.Message }
        };
}
