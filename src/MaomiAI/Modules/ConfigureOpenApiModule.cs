using Maomi;
using MaomiAI.Infra;
using MaomiAI.OpenApi;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace MaomiAI.Modules;

/// <summary>
/// 配置 OpenAPI .
/// </summary>
public class ConfigureOpenApiModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    public ConfigureOpenApiModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        var errorOpenApiMediaType = new Dictionary<string, OpenApiMediaType>
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["error"] = new OpenApiSchema { Type = "string" },
                        ["message"] = new OpenApiSchema { Type = "string" }
                    }
                }
            }
        };

        context.Services.AddOpenApi(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Maomi API",
                    Version = "v1",
                    Description = "MaomiAI openapi document."
                };

                document.Servers = new List<OpenApiServer>
                    {
                        new()
                        {
                            Url = _systemOptions.Server,
                            Description = "User-defined service address"
                        }
                    };

                return Task.CompletedTask;
            });

            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddSchemaTransformer<TypeSchemeTransformer>();



            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                operation.Responses.Add("500", new OpenApiResponse
                {
                    Description = "Internal server error",
                    Content = errorOpenApiMediaType
                });
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Internal server error",
                    Content = errorOpenApiMediaType
                });

                operation.Responses.Add("403", new OpenApiResponse
                {
                    Description = "Internal server error",
                    Content = errorOpenApiMediaType
                });

                return Task.CompletedTask;
            });
        });
    }
}
