// <copyright file="QueryServerInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Public.Queries.Response;
using MediatR;

namespace MaomiAI.Public.Queries;

/// <summary>
/// 查询服务器信息，将固定公开配置等返回给启动.
/// </summary>
public class QueryServerInfoCommand : IRequest<QueryServerInfoResponse>
{
}
