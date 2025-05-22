using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 站内信.
/// </summary>
public partial class MessageEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 主题.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// 内容.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 发送对象类型.
    /// </summary>
    public int SendObjectType { get; set; }

    /// <summary>
    /// 发送对象id.
    /// </summary>
    public Guid SendObjectId { get; set; }

    /// <summary>
    /// 接收对象类型.
    /// </summary>
    public int ReceiveObjectType { get; set; }

    /// <summary>
    /// 接收对象id.
    /// </summary>
    public Guid RecevieObjectId { get; set; }

    /// <summary>
    /// 已读.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 附带的地址.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// 地址标题.
    /// </summary>
    public string UrlTitle { get; set; } = default!;

    /// <summary>
    /// 消息类型.
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 软删除.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 更新时间.
    /// </summary>
    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 创建人.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 更新人.
    /// </summary>
    public Guid UpdateUserId { get; set; }
}
