using MaomiAI.AiModel.Shared.Models;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

public class QueryAiModelProviderListResponse
{
    /// <summary>
    /// AI 服务商列表，{ai服务提供商,模型数量}.
    /// </summary>
    public IReadOnlyCollection<KeyValuePair<string, int>> Providers { get; init; } = new Dictionary<string, int>();
}
