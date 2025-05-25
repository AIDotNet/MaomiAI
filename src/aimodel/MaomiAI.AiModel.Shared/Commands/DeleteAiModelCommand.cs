using MediatR;

namespace MaomiAI.AiModel.Shared.Commands;

/// <summary>
/// 删除 ai 模型.
/// </summary>
public class DeleteAiModelCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 模型 Id
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 模型 id.
    /// </summary>
    public Guid AiModelId { get; init; }
}
