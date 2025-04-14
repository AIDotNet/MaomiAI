// <copyright file="S3Controller.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Store.Controllers;

/// <summary>
/// 获取可公开访问的文件上传预签名地址.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/pre_public_url")]
[Authorize]
public class PreUploadEnpoint : Endpoint<PublicPreUploadFileCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    public PreUploadEnpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    ///// <inheritdoc/>
    //public override void Configure()
    //{
    //    Post($"{StoreApi.ApiPrefix}/pre_public_url");
    //}

    /// <inheritdoc/>
    public override Task<PreUploadFileCommandResponse> ExecuteAsync(PublicPreUploadFileCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}

/// <summary>
/// 完成文件上传.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/complate_url")]
[Authorize]
public class ComplateUploadEndpoint:Endpoint<ComplateFileUploadCommand, ComplateFileCommandResponse>
{
    private readonly IMediator _mediator;

    public ComplateUploadEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<ComplateFileCommandResponse> ExecuteAsync(ComplateFileUploadCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}

///// <summary>
///// S3 存储.
///// </summary>
//[ApiController]
//[Route("api/[controller]")]
//public class S3Controller : ControllerBase
//{
//    private readonly IMediator _mediator;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="S3Controller"/> class.
//    /// </summary>
//    /// <param name="mediator"></param>
//    public S3Controller(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    /// <summary>
//    /// 获取可公开访问的文件上传预签名地址.
//    /// </summary>
//    /// <param name="command"></param>
//    /// <returns>预签名地址.</returns>
//    [Microsoft.AspNetCore.Mvc.HttpPost("pre_public_url")]
//    [EndpointSummary("获取可公开访问的文件上传预签名地址.")]
//    [EndpointDescription("获取上传文件预签名地址，然后向 oss 地址上传文件.")]
//    public async Task<PreUploadFileCommandResponse> PreUploadTeamAvatarAsync([FromBody] PublicPreUploadFileCommand command)
//    {
//        var response = await _mediator.Send(command);
//        return response;
//    }

//    /// <summary>
//    /// 完成文件上传.
//    /// </summary>
//    /// <param name="command"></param>
//    /// <returns>.</returns>
//    [HttpPost("complate_url")]
//    [EndpointSummary("完成文件上传.")]
//    [EndpointDescription("完成文件上传.")]
//    public async Task<ComplateFileCommandResponse> ComplateUploadTeamAvatarAsync([FromBody] ComplateFileUploadCommand command)
//    {
//        var response = await _mediator.Send(command);
//        return response;
//    }
//}
