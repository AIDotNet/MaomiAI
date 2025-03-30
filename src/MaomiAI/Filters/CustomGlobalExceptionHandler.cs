using Maomi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaomiAI.Dtos;

namespace MaomiAI.Filters;

/// <summary>
/// 全局中间件未知异常拦截.<br />
/// MVC 异常拦截 <see cref="MVCExceptionFilter"/>.
/// </summary>
public class CustomGlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomGlobalExceptionHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomGlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public CustomGlobalExceptionHandler(ILogger<CustomGlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}",
            exceptionMessage, DateTime.UtcNow);

        // 后续其它中间件不再重复处理.
        return ValueTask.FromResult(true);
    }
}