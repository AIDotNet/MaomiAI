using Maomi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaomiAI.Dtos;

namespace MaomiAI.Filters;

/// <summary>
/// MVC异常处理.
/// </summary>
[InjectOn(Scheme = InjectScheme.None, Own = true)]
public class MVCExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<MVCExceptionFilter> _logger;
    private readonly IStringLocalizer _stringLocalizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MVCExceptionFilter"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="stringLocalizer"></param>
    public MVCExceptionFilter(ILogger<MVCExceptionFilter> logger, IStringLocalizer stringLocalizer)
    {
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    /// <inheritdoc/>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        // 未经处理的异常
        if (!context.ExceptionHandled)
        {
            var action = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;

            _logger.LogCritical(context.Exception,
                """
                    RequestId: {0}
                    ControllerName: {1}
                    ActionName: {2}
                    """,
                context.HttpContext.TraceIdentifier,
                action?.ControllerName,
                action?.ActionName
                );

            var response = new ErrorMessageResponse()
            {
                Code = 500,
                Message = _stringLocalizer["500", context.HttpContext.TraceIdentifier],
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500,
            };

            context.ExceptionHandled = true;
        }

        await Task.CompletedTask;
    }
}