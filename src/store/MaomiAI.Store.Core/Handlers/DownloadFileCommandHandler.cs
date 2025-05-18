// <copyright file="DownloadFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Queries;
using MaomiAI.Store.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Document.Core.Handlers.Documents;

public class DownloadFileCommandHandler : IRequestHandler<DownloadFileCommand, EmptyCommandResponse>
{
    private readonly IServiceProvider _serviceProvider;

    public DownloadFileCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<EmptyCommandResponse> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
    {
        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(request.Visibility);

        await fileStore.DownloadAsync(request.ObjectKey, request.FilePath);

        return EmptyCommandResponse.Default;
    }
}