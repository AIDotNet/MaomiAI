using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

public class QueryAiModelEndpointListResponse
{
    /// <summary>
    /// AI 模型列表.
    /// </summary>
    public IReadOnlyDictionary<Guid, AiNotKeyEndpoint> Providers { get; init; } = new Dictionary<Guid, AiNotKeyEndpoint>();
}