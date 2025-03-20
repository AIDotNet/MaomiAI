// <copyright file="BearerSecuritySchemeTransformer.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace MaomiAI
{
    public partial class MainModule
    {
        internal sealed class BearerSecuritySchemeTransformer(
            IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
        {
            public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
                CancellationToken cancellationToken)
            {
                IEnumerable<AuthenticationScheme>? authenticationSchemes =
                    await authenticationSchemeProvider.GetAllSchemesAsync();
                if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
                {
                    Dictionary<string, OpenApiSecurityScheme>? requirements = new()
                    {
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer", // "bearer" refers to the header name here
                            In = ParameterLocation.Header,
                            BearerFormat = "Json Web Token"
                        }
                    };
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes = requirements;

                    foreach (KeyValuePair<OperationType, OpenApiOperation> operation in
                             document.Paths.Values.SelectMany(path => path.Operations))
                    {
                        operation.Value.Security.Add(new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = "Bearer",
                                        Type = ReferenceType.SecurityScheme
                                    }
                                }
                            ] = Array.Empty<string>()
                        });
                    }
                }
            }
        }
    }
}