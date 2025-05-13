using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 用户是否有权限更新知识库.
/// </summary>
public class QueryCanUpdateWikiCommand : IRequest<bool>
{
    public Guid WikiId { get; init; }
    public Guid UserId { get; init; }
}
