// <copyright file="QueryFileDownloadUrlCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Store.Queries;

public class QueryFileDownloadUrlCommand : IRequest<QueryFileDownloadUrlCommandResponse>
{
    public TimeSpan ExpiryDuration { get; init; }
    public IReadOnlyCollection<string> ObjectKeys { get; init; } = new List<string>();
}

public class QueryFileDownloadUrlCommandResponse
{
    public IReadOnlyDictionary<string, Uri> Urls { get; init; } = new Dictionary<string, Uri>();
}
