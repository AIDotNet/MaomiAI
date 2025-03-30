using MaomiAI.Store.Commands;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Services;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Controllers;

/// <summary>
/// S3 存储.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="S3Controller"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public S3Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 获取可公开访问的文件上传预签名地址.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>预签名地址.</returns>
    [HttpPost("pre_public_url")]
    [EndpointSummary("获取可公开访问的文件上传预签名地址.")]
    [EndpointDescription("获取上传文件预签名地址，然后向 oss 地址上传文件.")]
    public async Task<PreUploadFileCommandResponse> PreUploadTeamAvatarAsync([FromBody] PublicPreUploadFileCommand command)
    {
        var response = await _mediator.Send(command);
        return response;
    }

    /// <summary>
    /// 获取可公开访问的文件上传预签名地址.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>预签名地址.</returns>
    [HttpPost("pre_private_url")]
    [EndpointSummary("获取私有访问的文件上传预签名地址.")]
    [EndpointDescription("获取上传文件预签名地址，然后向 oss 地址上传文件.")]
    public async Task<PreUploadFileCommandResponse> PreUploadTeamAvatarAsync([FromBody] PrivatePreUploadFileCommand command)
    {
        var response = await _mediator.Send(command);
        return response;
    }

    /// <summary>
    /// 完成文件上传.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>.</returns>
    [HttpPost("complate_url")]
    [EndpointSummary("完成文件上传.")]
    [EndpointDescription("完成文件上传.")]
    public async Task<ComplateFileCommandResponse> ComplateUploadTeamAvatarAsync([FromBody] ComplateFileUploadCommand command)
    {
        var response = await _mediator.Send(command);
        return response;
    }
}
