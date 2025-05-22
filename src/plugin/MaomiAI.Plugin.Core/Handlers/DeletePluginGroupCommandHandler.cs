using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Store.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Plugin.Core.Handlers;

public class DeletePluginGroupCommandHandler : IRequestHandler<DeletePluginGroupCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    public DeletePluginGroupCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    public async Task<EmptyCommandResponse> Handle(DeletePluginGroupCommand request, CancellationToken cancellationToken)
    {
        // 删除数据库
        // 删除 oss 文件
        var pluginGroup = await _databaseContext.TeamPluginGroups
            .FirstOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken);
        if (pluginGroup == null)
        {
            throw new BusinessException("分组不存在") { StatusCode = 404 };
        }

        await _mediator.Send(new DeleteFileCommand
        {
            FileId = pluginGroup.OpenapiFileId,
        });

        _databaseContext.TeamPluginGroups.Remove(pluginGroup);
        var pluginFiles = await _databaseContext.TeamPlugins.Where(x => x.GroupId == request.GroupId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.IsDeleted, true), cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
