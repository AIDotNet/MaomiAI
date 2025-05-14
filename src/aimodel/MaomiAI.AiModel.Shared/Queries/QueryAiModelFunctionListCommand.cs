using MaomiAI.AiModel.Shared.Models;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询某种用途的 AI 模型列表
/// </summary>
public class QueryAiModelFunctionListCommand : IRequest<QueryAiModelFunctionListCommandResponse>
{
    public Guid TeamId { get; init; }
    public AiModelFunction AiModelFunction { get; init; }
}

public class QueryAiModelFunctionListCommandResponse
{
    /// <summary>
    /// AI 模型列表.
    /// </summary>
    public IReadOnlyCollection<AiNotKeyEndpoint> AiModels { get; init; } = new List<AiNotKeyEndpoint>();
}