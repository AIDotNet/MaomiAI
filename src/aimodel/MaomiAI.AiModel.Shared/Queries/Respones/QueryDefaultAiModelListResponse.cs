using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

public class QueryDefaultAiModelListResponse
{
    public IReadOnlyCollection<AiNotKeyEndpoint> AiModels { get; init; } = default!;
}
