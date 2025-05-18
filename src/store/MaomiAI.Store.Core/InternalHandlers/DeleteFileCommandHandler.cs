// <copyright file="DeleteFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Commands;
using MediatR;

namespace MaomiAI.Store.InternalHandlers;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, EmptyCommandResponse>
{
    public async Task<EmptyCommandResponse> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        return new EmptyCommandResponse();
    }
}