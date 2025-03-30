using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 完成文件上传.
/// </summary>
public class ComplateFileCommandHandler : IRequestHandler<ComplateFileUploadCommand, ComplateFileCommandResponse>
{
    private readonly MaomiaiContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateFileCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="serviceProvider"></param>
    public ComplateFileCommandHandler(MaomiaiContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<ComplateFileCommandResponse> Handle(ComplateFileUploadCommand request, CancellationToken cancellationToken)
    {
        var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == request.FileId, cancellationToken);

        if (file == null)
        {
            throw new BusinessException("文件不存在");
        }

        if (file.IsUpload)
        {
            return new ComplateFileCommandResponse();
        }

        // 无论成功失败，都先检查对象存储文件是否存在
        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(file.IsPublic ? FileStoreType.Public : FileStoreType.Private);
        var fileSize = await fileStore.GetFileSizeAsync(file.Path);

        if (request.IsSuccess)
        {
            if (fileSize != file.FileSize)
            {
                _dbContext.Files.Remove(file);
                await _dbContext.SaveChangesAsync();

                throw new BusinessException("上传的文件已损坏");
            }

            file.IsUpload = true;
            _dbContext.Update(file);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }

        return new ComplateFileCommandResponse();
    }
}
