using MaomiAI.AI.Models;
using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Chat.Shared.Commands.Responses;
using MaomiAI.Infra.Models;
using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaomiAI.Chat.Core.Handlers;

/// <summary>
/// 更新对话信息.
/// </summary>
public class UpdateChatCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 每个聊天对话都有唯一 id.
    /// </summary>
    public Guid ChatId { get; init; } = default!;

    /// <summary>
    /// 要使用的 AI 模型，该模型必须在对应的团队中.
    /// </summary>
    public Guid ModelId { get; init; }

    /// <summary>
    /// 要使用的知识库，知识库如果不在该团队中，则必须是公开的.
    /// </summary>
    public Guid? WikiId { get; init; }

    /// <summary>
    /// 话题名称.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// 要使用的插件 id 列表.
    /// </summary>
    public IReadOnlyCollection<Guid> PluginIds { get; init; } = new List<Guid>();

    /// <summary>
    /// 历史对话或者上下文信息，创建对话时，如果有提示词，则第一个对话就是提示词.
    /// </summary>
    public ChatHistory ChatHistory { get; init; } = new ChatHistory();

    /// <summary>
    /// 配置，字典适配不同的 AI 模型.
    /// </summary>
    public IReadOnlyCollection<AiExecutionSetting> ExecutionSettings { get; init; } = new List<AiExecutionSetting>();
}