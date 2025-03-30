// <copyright file="CreateTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FluentValidation;
using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 创建团队命令.
/// </summary>
public class CreateTeamCommand : IRequest<GuidDto>
{
    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 是否允许被外部搜索.
    /// </summary>
    public bool IsPublic { get; set; }
}

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("团队名称最大长度20.");
        RuleFor(x => x.Description).MaximumLength(255).WithMessage("团队描述最大长度255.");
    }
}
