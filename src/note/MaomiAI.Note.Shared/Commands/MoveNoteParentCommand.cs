using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Note.Commands;

/// <summary>
/// 移动笔记到其他父级.
/// </summary>
public class MoveNoteParentCommand: IRequest<EmptyCommandResponse>
{
    public Guid NoteId { get; init; }
    public Guid ParentId { get; init; }
}