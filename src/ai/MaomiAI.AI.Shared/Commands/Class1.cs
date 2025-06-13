using MaomiAI.AI.Models;
using MaomiAI.AiModel.Shared.Models;
using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaomiAI.AI.Commands;

public class ChatCompletionsCommand: IStreamRequest<OpenAIChatCompletionsObject>
{
    /// <summary>
    /// Chat id.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 对话 AI 信息.
    /// </summary>
    public AiEndpoint Endpoint { get; init; }

    /// <summary>
    /// 历史对话或者上下文信息，创建对话时，如果有提示词，则第一个对话就是提示词.
    /// </summary>
    public ChatHistory ChatHistory { get; init; } = new ChatHistory();

    /// <summary>
    /// 插件列表.
    /// </summary>
    public IReadOnlyCollection<KernelPlugin> Plugins { get; init; } = new List<KernelPlugin>();

    /// <summary>
    /// 执行属性信息.
    /// </summary>
    public IReadOnlyCollection<AiExecutionSetting> ExecutionSetting { get;init; } = new List<AiExecutionSetting>();
}