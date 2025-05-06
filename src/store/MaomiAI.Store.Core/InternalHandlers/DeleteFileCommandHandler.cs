using MaomiAI.Store.Commands;
using MediatR;

namespace MaomiAI.Store.InternalHandlers;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, EmptyCommandResponse>
{
    public async Task<EmptyCommandResponse> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        return new EmptyCommandResponse();
    }
}