// <copyright file="DeleteWikiDocumentCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Commands;
using MediatR;

namespace MaomiAI.Document.Core.Handlers;

public class DeleteWikiDocumentCommandHandler : IRequestHandler<DeleteWikiDocumentCommand, EmptyCommandResponse>
{
    public Task<EmptyCommandResponse> Handle(DeleteWikiDocumentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}