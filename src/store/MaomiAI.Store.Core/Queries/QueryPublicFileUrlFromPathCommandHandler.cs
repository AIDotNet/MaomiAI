// <copyright file="QueryPublicFileUrlFromPathCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra;
using MaomiAI.Store.Queries.Response;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 通过 path/objectkey 查询公有文件的访问路径.
/// </summary>
public class QueryPublicFileUrlFromPathCommandHandler : IRequestHandler<QueryPublicFileUrlFromPathCommand, QueryPublicFileUrlFromPathResponse>
{
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPublicFileUrlFromPathCommandHandler"/> class.
    /// </summary>
    /// <param name="systemOptions"></param>
    public QueryPublicFileUrlFromPathCommandHandler(SystemOptions systemOptions)
    {
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<QueryPublicFileUrlFromPathResponse> Handle(QueryPublicFileUrlFromPathCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var endpoint = new Uri(_systemOptions.PublicStore.Endpoint);
        if (!_systemOptions.PublicStore.ForcePathStyle)
        {
            endpoint = new Uri($"{endpoint.Scheme}://{_systemOptions.PublicStore.Bucket}.{endpoint.Host}");
        }
        else
        {
            endpoint = new Uri($"{endpoint.Scheme}://{endpoint.Host}/{_systemOptions.PublicStore.Bucket}");
        }

        Dictionary<string, string> urls = new();
        foreach (var objectKey in request.ObjectKeys.Distinct())
        {
            if (string.IsNullOrWhiteSpace(objectKey))
            {
                continue;
            }
            urls.Add(objectKey, new Uri(endpoint, objectKey).ToString());
        }

        return new QueryPublicFileUrlFromPathResponse()
        {
            Urls = urls
        };
    }
}
