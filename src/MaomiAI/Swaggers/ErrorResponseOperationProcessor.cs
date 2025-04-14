using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using NSwag;
using MaomiAI.Infra.Models;

namespace MaomiAI.Swaggers;

public class ErrorResponseOperationProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        var responseSchema = context.SchemaGenerator.Generate(typeof(ErrorResponse), context.SchemaResolver);

        var response = new OpenApiResponse
        {
            Description = "An unexpected error occurred.",
        };

        response.Content["application/json"] = new OpenApiMediaType
        {
            Schema = responseSchema
        };

        context.OperationDescription.Operation.Responses["500"] = response;
        context.OperationDescription.Operation.Responses["400"] = response;
        context.OperationDescription.Operation.Responses["401"] = response;
        context.OperationDescription.Operation.Responses["403"] = response;
        return true;
    }
}
