using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 删除文件.
/// </summary>
public class InternalDeleteFileCommand : IRequest<EmptyCommandResponse>
{
    public Guid FileId { get; init; }
}
