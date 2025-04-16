using MaomiAI.Database;
using MaomiAI.Store.Queries;
using MaomiAI.Store.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 检查文件是否存在.
/// </summary>
public class CheckFileExistCommandHandler : IRequestHandler<CheckFileExistCommand, CheckFileExistCommandResponse>
{
    private readonly MaomiaiContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckFileExistCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="serviceProvider"></param>
    public CheckFileExistCommandHandler(MaomiaiContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<CheckFileExistCommandResponse> Handle(CheckFileExistCommand request, CancellationToken cancellationToken)
    {
        var isPublicFile = request.Visibility == Enums.FileVisibility.Public ? true : false;

        var query = _dbContext.Files.Where(x => x.IsPublic == isPublicFile);
        if (request.FileId != null)
        {
            query = query.Where(x => x.Id == request.FileId);
        }

        if (!string.IsNullOrEmpty(request.MD5))
        {
            query = query.Where(x => x.FileMd5 == request.MD5);
        }

        if (!string.IsNullOrEmpty(request.Key))
        {
            query = query.Where(x => x.Path == request.Key);
        }

        var existFile = await query.AnyAsync(cancellationToken);
        return new CheckFileExistCommandResponse
        {
            Exist = existFile
        };
    }
}