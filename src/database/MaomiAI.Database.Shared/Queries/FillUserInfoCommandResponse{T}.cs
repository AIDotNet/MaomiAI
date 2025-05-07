// <copyright file="FillUserInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Database.Queries;

public class FillUserInfoCommandResponse
{
    public IReadOnlyCollection<AuditsInfo> Items { get; init; }
}