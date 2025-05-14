// <copyright file="SetEmbeddingGenerationDocumentTaskCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;
using Maomi.MQ;
using MaomiAI.AiModel.Shared.Helpers;
using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Document.Shared.Models;
using MaomiAI.Infra;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Service;
using MaomiAI.Store.Clients;
using MaomiAI.Store.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace MaomiAI.Document.Core.Consumers.Events;

// 发送消息，后台执行任务

public class EmbeddingDocumentEvent
{
    public Guid TeamId { get; init; }
    public Guid WikiId { get; init; }
    public Guid DocumentId { get; init; }
    public string TaskId { get; init; }
}
