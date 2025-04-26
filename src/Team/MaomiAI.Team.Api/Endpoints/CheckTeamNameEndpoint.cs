// <copyright file="CreateTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Database;
using MaomiAI.Team.Shared.Commands;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Api.Endpoints;

/// <summary>
/// 检查团队名称是否存在.
/// </summary>
[EndpointGroupName("team")]
[FastEndpoints.HttpPost($"{TeamApi.ApiPrefix}/check_name")]
public class CheckTeamNameEndpoint : Endpoint<CheckTeamNameCommand, ExistResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckTeamNameEndpoint"/> class.
    /// </summary>
    /// <param name="maomiaiContext"></param>
    public CheckTeamNameEndpoint(DatabaseContext maomiaiContext)
    {
        _dbContext = maomiaiContext;
    }

    /// <inheritdoc/>
    public override async Task<ExistResponse> ExecuteAsync(CheckTeamNameCommand req, CancellationToken ct)
    {
        var query = _dbContext.Teams.Where(x => x.Name == req.Name);
        if (req.Id != null)
        {
            query = query.Where(x => x.Id != req.Id);
        }

        var existRecord = await query.AnyAsync();

        return new ExistResponse
        {
            IsExist = existRecord
        };
    }
}