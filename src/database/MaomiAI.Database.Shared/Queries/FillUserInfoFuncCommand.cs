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
/// <typeparam name="T">带有审计属性的.</typeparam>
public class FillUserInfoFuncCommand<T> : IRequest<ICollection<T>>
{
    public ICollection<T> Items { get; init; }

    public Func<ICollection<T>, ICollection<Guid>> GetUserId { get; init; }
    public Action<IReadOnlyDictionary<Guid, string>, T> SetUserName { get; init; }
}
