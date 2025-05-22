// <copyright file="UpdatePluginInfoCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using System.Data.Entity;
using System.Transactions;

namespace MaomiAI.Plugin.Core.Handlers;

public class UpdatePluginInfoCommandHandler : IRequestHandler<UpdatePluginInfoCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    public UpdatePluginInfoCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<EmptyCommandResponse> Handle(UpdatePluginInfoCommand request, CancellationToken cancellationToken)
    {
        var pluginGroup = await _databaseContext.TeamPluginGroups
            .FirstOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken);
        if (pluginGroup == null)
        {
            throw new BusinessException("分组不存在") { StatusCode = 404 };
        }

        pluginGroup.Name = request.Name;
        pluginGroup.Server = request.ServerUrl;
        pluginGroup.Header = request.Header;
        pluginGroup.Description = request.Description;
        _databaseContext.TeamPluginGroups.Update(pluginGroup);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}