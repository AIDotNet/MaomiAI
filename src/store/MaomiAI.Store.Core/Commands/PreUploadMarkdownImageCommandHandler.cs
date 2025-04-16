using Maomi.AI.Exceptions;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.InternalCommands;
using MaomiAI.Team.Shared.Commands;
using MaomiAI.Team.Shared.Helpers;
using MediatR;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 预上传 Markdown 图像.
/// </summary>
public class PreUploadMarkdownImageCommandHandler : IRequestHandler<PreUploadMarkdownImageCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadMarkdownImageCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadMarkdownImageCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<PreUploadFileCommandResponse> Handle(PreUploadMarkdownImageCommand request, CancellationToken cancellationToken)
    {
        if (FileStoreHelper.ImageExtensions.Contains(request.FileName.Split('.').Last()))
        {
            throw new BusinessException("文件格式不正确");
        }

        // todo: 限制头像文件大小.

        var preu = new PreuploadFileCommand
        {
            FileName = request.FileName,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            MD5 = request.MD5,
            Expiration = TimeSpan.FromMinutes(1),
            Visibility = Enums.FileVisibility.Public,
            Path = FileStoreHelper.GetObjectKey(request.MD5, request.FileName)
        };

        return await _mediator.Send(preu, cancellationToken);
    }
}
