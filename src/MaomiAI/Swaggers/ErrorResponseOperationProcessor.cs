using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace MaomiAI.Swaggers;

public class ErrorResponseOperationProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        var responseSchema = context.SchemaGenerator.Generate(typeof(MaomiAI.Infra.Models.ErrorResponse), context.SchemaResolver);

        var response = new OpenApiResponse
        {
            Description = "An unexpected error occurred.",
        };

        response.Content["application/json"] = new OpenApiMediaType
        {
            Schema = responseSchema
        };

        context.OperationDescription.Operation.Responses.Remove("400");

        context.OperationDescription.Operation.Responses["500"] = response;
        context.OperationDescription.Operation.Responses["400"] = response;
        context.OperationDescription.Operation.Responses["401"] = response;
        context.OperationDescription.Operation.Responses["403"] = response;
        context.OperationDescription.Operation.Responses["409"] = response;

        return true;
    }
}
