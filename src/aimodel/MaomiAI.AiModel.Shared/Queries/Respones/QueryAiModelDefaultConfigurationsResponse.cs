using MaomiAI.AiModel.Shared.Models;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

public class QueryAiModelDefaultConfigurationsResponse
{
    public AiModelDefaultConfiguration DefaultConfigurations { get; init; } = default!;
}
