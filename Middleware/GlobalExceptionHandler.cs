using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProsjektOppgave_AdeleTjoennaas.Middleware {

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

_logger.LogError(
    exception,
    "Ubehandlet feil: {Message} (Path: {Path}, Method: {Method})",
    exception.Message,
    httpContext.Request.Path,
    httpContext.Request.Method
);

    
        var (status, title, detail) = exception switch
        {
            ArgumentException argEx => (StatusCodes.Status400BadRequest, "Bad Request", argEx.Message),
            
            HttpRequestException => ( StatusCodes.Status503ServiceUnavailable,
                     "Eksternt API utilgjengelig",
                    "Kunne ikke hente data fra Azure API."),

            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found", "The requested resource was not found."),

            _ => (StatusCodes.Status500InternalServerError, "Server Error", "An unexpected error occurred.")
        };

        ProblemDetails problem = new ProblemDetails
        {
            Status = status,
            Title  = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true; 
    }}}