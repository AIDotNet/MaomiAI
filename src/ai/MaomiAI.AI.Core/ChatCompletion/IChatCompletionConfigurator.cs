// <copyright file="Class1.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using Microsoft.SemanticKernel;

namespace MaomiAI.AI.Core.ChatCompletion;

public interface IChatCompletionConfigurator
{
    IKernelBuilder AddChatCompletion(IKernelBuilder kernelBuilder, AiEndpoint endpoint);
}
