// <copyright file="QueryUserIsTeamAdminCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Team.Shared.Queries.Responses;

public class QueryUserIsTeamAdminCommandResponse
{
    public bool IsAdmin { get; set; }
    public bool IsOwner { get; init; }
}
