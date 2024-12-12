using LetsMeet.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Infrastructure;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Task exceptionHandler = exception switch
        {
            AppException appException => HandleAppException(httpContext, appException),
            _ => HandleUnhandledException(httpContext, exception)
        };

        await exceptionHandler;
        return true;
    }

    private async Task HandleAppException(HttpContext httpContext, AppException exception)
    {
        httpContext.Response.StatusCode = (int)exception.ErrorCode;

        var problemDetails = new ProblemDetails
        {
            Status = (int)exception.ErrorCode,
            Title = exception.Message,
            Type = exception.Type
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }

    private async Task HandleUnhandledException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = exception.HResult;

        var problemDetails = new ProblemDetails
        {
            Status = exception.HResult,
            Title = exception.Message,
            Type = exception.GetType().ToString()
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}