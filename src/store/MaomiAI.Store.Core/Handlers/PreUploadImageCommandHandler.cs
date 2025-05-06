using MaomiAI.Store.Commands.Response;
using MaomiAI.Team.Shared.Commands;
using MaomiAI.Team.Shared.Helpers;
using MediatR;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 预上传图片，存储时设置为公开.
/// </summary>
public class PreUploadImageCommandHandler : IRequestHandler<PreUploadImageCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadImageCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadImageCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<PreUploadFileCommandResponse> Handle(PreUploadImageCommand request, CancellationToken cancellationToken)
    {
        var fileExtension = Path.GetExtension(request.FileName);
        if (!FileStoreHelper.ImageExtensions.Contains(fileExtension))
        {
            throw new BusinessException("不支持该类型的图像") { StatusCode = 400};
        }

        // todo: 通过系统设置限制头像文件大小.

        var preu = new PreuploadFileCommand
        {
            FileName = request.FileName,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            MD5 = request.MD5,
            Expiration = TimeSpan.FromMinutes(1),
            Visibility = Enums.FileVisibility.Public,
            ObjectKey = FileStoreHelper.GetObjectKey(request.MD5, request.FileName),
        };

        return await _mediator.Send(preu, cancellationToken);
    }
}
