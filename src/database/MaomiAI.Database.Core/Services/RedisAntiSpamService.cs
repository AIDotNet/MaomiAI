// <copyright file="RedisAntiSpamService.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MaomiAI.Infra.Services;

/// <summary>
/// 基于 Redis 的防刷服务.
/// </summary>
public class RedisAntiSpamService
{
    private readonly IRedisDatabase _redisDatabase;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisAntiSpamService"/> class.
    /// </summary>
    /// <param name="redisDatabase"></param>
    public RedisAntiSpamService(IRedisDatabase redisDatabase)
    {
        _redisDatabase = redisDatabase;
    }

    /// <summary>
    /// 检查客户端请求是否超出限制.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="clientInfo">客户端信息.</param>
    /// <param name="seconds">时间窗口（秒）.</param>
    /// <param name="count">最大请求次数.</param>
    /// <returns>是否允许请求.</returns>
    public async Task<bool> IsRequestAllowed(string key, ClientInfo clientInfo, int seconds, int count)
    {
        string clientKey = $"antispam:{key}";

        // 使用 Redis 的 INCR 和 EXPIRE 实现计数和过期
        var transaction = _redisDatabase.Database.CreateTransaction();
        var currentCount = await transaction.StringIncrementAsync(clientKey);
        await transaction.KeyExpireAsync(clientKey, TimeSpan.FromSeconds(seconds));
        await transaction.ExecuteAsync();

        return currentCount <= count;
    }
}