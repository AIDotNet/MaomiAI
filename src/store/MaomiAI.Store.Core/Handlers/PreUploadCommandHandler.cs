//// <copyright file="PreUploadTeamAvatarCommandHandler.cs" company="MaomiAI">
//// Copyright (c) MaomiAI. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//// Github link: https://github.com/AIDotNet/MaomiAI
//// </copyright>

//using MaomiAI.Store.Commands.Response;
//using MaomiAI.Store.InternalCommands;
//using MaomiAI.Team.Shared.Commands;
//using MaomiAI.Team.Shared.Helpers;
//using MediatR;

//namespace MaomiAI.Store.Commands;

///// <summary>
///// 获取上传私有文件的预签名地址.
///// </summary>
//public class PreUploadCommandHandler : IRequestHandler<PreUploadFileCommand, PreUploadFileCommandResponse>
//{
//    private readonly IMediator _mediator;

//    public PreUploadCommandHandler(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    public async Task<PreUploadFileCommandResponse> Handle(PreUploadFileCommand request, CancellationToken cancellationToken)
//    {
//        var preu = new PreuploadFileCommand
//        {
//            FileName = request.FileName,
//            ContentType = request.ContentType,
//            FileSize = request.FileSize,
//            MD5 = request.MD5,
//            Expiration = TimeSpan.FromMinutes(1),
//            Visibility = request.Visibility,
//            Path = FileStoreHelper.GetObjectKey(request.MD5, request.FileName)
//        };

//        var response = await _mediator.Send(preu, cancellationToken);
//        return new PreUploadFileCommandResponse
//        {
//            IsExist = response.IsExist,
//            FileId = response.FileId,
//            Expiration = response.Expiration,
//            UploadUrl = response.UploadUrl
//        };
//    }
//}
