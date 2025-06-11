// <copyright file="CreateNoteEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Note.Commands;
using MaomiAI.Note.Queries;
using MaomiAI.Note.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Note.Endpoints;

/// <summary>
/// 新建一个笔记.
/// </summary>
[EndpointGroupName("note")]
[FastEndpoints.HttpPost($"{NoteApi.ApiPrefix}/create")]
public class CreateNoteEndpoint : Endpoint<CreateNoteCommand, IdResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNoteEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public CreateNoteEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<IdResponse> ExecuteAsync(CreateNoteCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}

/// <summary>
/// 删除一个笔记.
/// </summary>
[EndpointGroupName("note")]
[FastEndpoints.HttpDelete($"{NoteApi.ApiPrefix}/delete")]
public class DeleteNoteEndpoint : Endpoint<DeleteNoteCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteNoteEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public DeleteNoteEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(DeleteNoteCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}

/// <summary>
/// 移动笔记目录.
/// </summary>
[EndpointGroupName("note")]
[FastEndpoints.HttpPost($"{NoteApi.ApiPrefix}/move_note")]
public class MoveNoteParentEndpoint : Endpoint<MoveNoteParentCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoveNoteParentEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public MoveNoteParentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(MoveNoteParentCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}

/// <summary>
/// 更新笔记内容.
/// </summary>
[EndpointGroupName("note")]
[FastEndpoints.HttpPost($"{NoteApi.ApiPrefix}/update")]
public class UpdateNoteEndpoint : Endpoint<UpdateNoteCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateNoteEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UpdateNoteEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(UpdateNoteCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}

// todo: 后续重新设计路由
/// <summary>
/// 获取笔记内容.
/// </summary>
[EndpointGroupName("note")]
[FastEndpoints.HttpGet($"{NoteApi.ApiPrefix}/{"{noteId}"}")]
public class QueryNoteEndpoint : Endpoint<QueryNoteCommand, QueryNoteCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryNoteEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryNoteEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryNoteCommandResponse> ExecuteAsync(QueryNoteCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}

/// <summary>
/// 获取目录树.
/// </summary>
[EndpointGroupName("note")]
[FastEndpoints.HttpPost($"{NoteApi.ApiPrefix}/catalog")]
public class CatalogEndpoint : Endpoint<QueryNoteTreeCommand, QueryNoteTreeCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public CatalogEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryNoteTreeCommandResponse> ExecuteAsync(QueryNoteTreeCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}
