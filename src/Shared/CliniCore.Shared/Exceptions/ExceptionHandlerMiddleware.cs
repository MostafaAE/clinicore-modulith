using Microsoft.AspNetCore.Http;
using System.Net;

namespace CliniCore.Shared.Exceptions;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ClinicoreException ex)
        {
            // Handle custom exceptions
            await HandleCustomExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            await HandleExceptionAsync(context, ex);
        }

    }

    private async Task HandleCustomExceptionAsync(HttpContext context, ClinicoreException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex.StatusCode;

        var errorResponse = new ErrorResponse
        {
            StatusCode = ex.StatusCode,
            Errors = new List<ErrorMessage> { new ErrorMessage { Message = ex.Message } }
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Errors = new List<ErrorMessage> { new ErrorMessage { Message = "Something went wrong, please contact your administrator." } }
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}