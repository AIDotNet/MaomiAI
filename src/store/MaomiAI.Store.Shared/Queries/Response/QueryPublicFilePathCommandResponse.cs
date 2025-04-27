// <copyright file="QueryPublicFilePathCommandResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Store.Queries.Response;

public class QueryPublicFilePathCommandResponse
{
    public bool Exist { get; init; }

    public string Url { get; init; }

    public string Path { get; init; }
}