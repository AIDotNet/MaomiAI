// <copyright file="FillUserInfoFuncCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Database.Queries;

/// <summary>
/// 用户信息查询填充.
/// </summary>
public class FillUserInfoFuncCommand : IRequest<FillUserInfoFuncCommandResponse>
{
    public IReadOnlyCollection<object> Items { get; init; }

    public Func<IReadOnlyCollection<object>, IReadOnlyCollection<Guid>> GetUserId { get; init; }
    public Action<IReadOnlyDictionary<Guid, string>, object> SetUserName { get; init; }
}


public class FillUserInfoFuncCommandResponse
{
    public IReadOnlyCollection<object> Items { get; init; }

}