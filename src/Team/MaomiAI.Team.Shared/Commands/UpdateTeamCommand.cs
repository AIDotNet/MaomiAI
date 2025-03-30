// <copyright file="UpdateTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 更新团队命令.
/// </summary>
public class UpdateTeamCommand : IRequest
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 团队头像URL.
    /// </summary>
    public string? Avatar { get; set; }
}

public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("团队名称最大长度20.");
        RuleFor(x => x.Description).MaximumLength(255).WithMessage("团队描述最大长度255.");
    }
}