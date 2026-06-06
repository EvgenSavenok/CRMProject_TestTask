using System.Text.Json;
using Contacts.Domain.ErrorHandlers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Infrastrucure.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            AlreadyExistsException => StatusCodes.Status409Conflict,
            BadRequestException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        var detail = exception is ValidationException ve
            ? string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))
            : exception.Message;

        var details = new ProblemDetails
        {
            Title = "Error",
            Type = exception.GetType().Name,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(details));
    }
}
