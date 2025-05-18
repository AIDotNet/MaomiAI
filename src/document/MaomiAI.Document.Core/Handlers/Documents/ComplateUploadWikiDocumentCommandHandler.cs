// <copyright file="ComplateUploadWikiDocumentCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands.Documents;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Commands.Response;
using MediatR;

namespace MaomiAI.Document.Core.Handlers.Documents;

/// <summary>
/// 完成上传文档.
/// </summary>
public class ComplateUploadWikiDocumentCommandHandler : IRequestHandler<ComplateUploadWikiDocumentCommand, ComplateFileCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateUploadWikiDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="databaseContext"></param>
    public ComplateUploadWikiDocumentCommandHandler(IMediator mediator, DatabaseContext databaseContext)
    {
        _mediator = mediator;
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<ComplateFileCommandResponse> Handle(ComplateUploadWikiDocumentCommand request, CancellationToken cancellationToken)
    {
        _ = await _mediator.Send(new ComplateFileUploadCommand
        {
            FileId = request.FileId,
            IsSuccess = request.IsSuccess,
        });

        return new ComplateFileCommandResponse();
    }
}
