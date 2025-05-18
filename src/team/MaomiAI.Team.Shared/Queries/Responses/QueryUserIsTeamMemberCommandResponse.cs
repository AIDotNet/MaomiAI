// <copyright file="QueryUserIsTeamMemberCommandResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Team.Shared.Queries.Responses;

public class QueryUserIsTeamMemberCommandResponse
{
    public bool IsPublic { get; init; }
    public bool IsMember { get; init; }
    public bool IsOwner { get; init; }
    public bool IsAdmin { get; init; }
}