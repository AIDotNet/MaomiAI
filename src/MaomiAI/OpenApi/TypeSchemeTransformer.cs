// <copyright file="TypeSchemeTransformer.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace MaomiAI
{
    public partial class MainModule
    {
        internal sealed class TypeSchemeTransformer : IOpenApiSchemaTransformer
        {
            public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
                CancellationToken cancellationToken)
            {
                // 后续支持其它类型转换文档
                if (context.JsonTypeInfo.Type == typeof(decimal))
                {
                    schema.Format = "decimal";
                }
                else if (context.JsonTypeInfo.Type == typeof(DateTimeOffset))
                {
                    schema.Format = "string";
                }

                return Task.CompletedTask;
            }
        }
    }
}