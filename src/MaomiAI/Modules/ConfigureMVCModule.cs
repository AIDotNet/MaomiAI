using FluentValidation;
using Maomi;
using MaomiAI.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MaomiAI.Modules;

/// <summary>
/// 配置 MVC .
/// </summary>
public class ConfigureMVCModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureMVCModule"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public ConfigureMVCModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        //context.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

        //context.Services.AddControllers(options =>
        //{
        //    options.Filters.Add<MaomiExceptionFilter>();
        //})
        //.ConfigureApplicationPartManager(apm =>
        //{
        //    //foreach (var assembly in context.Modules.Select(x => x.Assembly).Distinct())
        //    //{
        //    //    if (assembly.GetName()?.Name?.EndsWith(".api", StringComparison.CurrentCultureIgnoreCase) != true)
        //    //    {
        //    //        continue;
        //    //    }

        //    //    apm.ApplicationParts.Add(new AssemblyPart(assembly));
        //    //}
        //});

        context.Services.AddValidatorsFromAssemblies(context.Modules.Select(x => x.Assembly).Distinct());

        //context.Services.AddFluentValidationAutoValidation();
    }
}
//public class CustomProblemDetailsFactory : ProblemDetailsFactory
//{
//    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
//    {
//        return base.CreateProblemDetails
//    }

//    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
//    {
//    }
//}