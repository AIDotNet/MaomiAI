// <copyright file="ErrorResponseOperationProcessor.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace MaomiAI.Swaggers;

/// <summary>
/// Swagger 统一响应处理.
/// </summary>
public class ErrorResponseOperationProcessor : IOperationProcessor
{
    /// <inheritdoc/>
    public bool Process(OperationProcessorContext context)
    {
        var responseSchema = context.SchemaGenerator.Generate(typeof(BusinessErrorResponse), context.SchemaResolver);
        var response = new OpenApiResponse
        {
            Description = "An error occurred in the request.",
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
