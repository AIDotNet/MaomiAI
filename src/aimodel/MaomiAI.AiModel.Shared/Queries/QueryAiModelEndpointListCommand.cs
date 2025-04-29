using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询 AI 模型端点列表.
/// </summary>
public class QueryAiModelEndpointListCommand : IRequest<QueryAiModelEndpointListResponse>
{
    public Guid TeamId { get; init; }
    public AiProvider Provider { get; init; }
}
