using MaomiAI.AI.Models;
using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MaomiAI.Chat.Shared.Commands;

// 检查信息，将知识库和插件传递到对话模块中，
public class ChatProcessingCommand : IStreamRequest<string>
{
    /// <summary>
    /// Chat id.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 历史对话或者上下文信息，创建对话时，如果有提示词，则第一个对话就是提示词.
    /// </summary>
    public ChatHistory ChatHistory { get; init; } = new ChatHistory();

    /// <summary>
    /// 执行属性信息.
    /// </summary>
    public IReadOnlyCollection<AiExecutionSetting> ExecutionSetting { get; init; } = new List<AiExecutionSetting>();
}
