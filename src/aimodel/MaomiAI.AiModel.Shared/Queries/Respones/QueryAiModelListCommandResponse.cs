using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

public class QueryAiModelListCommandResponse
{
    /// <summary>
    /// AI 模型列表.
    /// </summary>
    public IReadOnlyCollection<AiNotKeyEndpoint> AiModels { get; init; } = new List<AiNotKeyEndpoint>();
}