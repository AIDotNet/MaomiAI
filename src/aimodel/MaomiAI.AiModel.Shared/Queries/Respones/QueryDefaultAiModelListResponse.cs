using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

public class QueryDefaultAiModelListResponse
{
    public IReadOnlyCollection<AiModelDefaultConfiguration> AiModels { get; init; } = default!;
}
