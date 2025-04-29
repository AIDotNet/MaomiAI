// <copyright file="UploadTeamAvatarCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Commands.Root;

public class UploadTeamAvatarCommand : IRequest<EmptyCommandResponse>
{
    public Guid TeamId { get; init; }
    public Guid FileId { get; init; }
}
