using Microsoft.AspNetCore.Builder;

namespace CliniCore.Shared.Exceptions;

internal static class Extensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlerMiddleware>();
}