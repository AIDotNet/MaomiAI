using MaomiAI.Store.Commands;
using MediatR;

namespace MaomiAI.Store.InternalHandlers;

public class InternalDeleteFileCommandHandler : IRequestHandler<InternalDeleteFileCommand, EmptyCommandResponse>
{
    public async Task<EmptyCommandResponse> Handle(InternalDeleteFileCommand request, CancellationToken cancellationToken)
    {
        return new EmptyCommandResponse();
    }
}