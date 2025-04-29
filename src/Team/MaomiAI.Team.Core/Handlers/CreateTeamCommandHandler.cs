// <copyright file="CreateTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Handlers;

/// <summary>
/// 处理创建团队命令.
/// </summary>
public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, GuidResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<CreateTeamCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTeamCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="userContext"></param>
    public CreateTeamCommandHandler(
        DatabaseContext dbContext,
        ILogger<CreateTeamCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        Guid currentUserId = _userContext.UserId;

        var existTeam = await _dbContext.Teams.AnyAsync(x => x.Name == request.Name, cancellationToken);
        if (existTeam)
        {
            throw new BusinessException("团队名称已存在.");
        }

        var team = new TeamEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsPublic = false,
            CreateUserId = currentUserId,
            CreateTime = DateTime.Now
        };

        await _dbContext.Teams.AddAsync(team, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("The user creates a team,{@Team}.", team);
        return new GuidResponse { Guid = team.Id };
    }
}