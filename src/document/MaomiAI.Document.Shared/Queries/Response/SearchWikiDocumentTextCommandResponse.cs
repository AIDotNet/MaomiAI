// <copyright file="SearchWikiDocumentTextCommandResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.KernelMemory;

namespace MaomiAI.Document.Core.Handlers.Responses;

public class SearchWikiDocumentTextCommandResponse
{
    public SearchResult SearchResult { get; init; }
}