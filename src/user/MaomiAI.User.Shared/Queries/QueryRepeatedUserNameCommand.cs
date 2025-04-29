// <copyright file="QueryRepeatedUserNameCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Queries;

public class QueryRepeatedUserNameCommand : IRequest<Simple<bool>>
{
    public string UserName { get; set; } = string.Empty;
}