﻿using Microsoft.Extensions.AI;

namespace MaomiAI.AI.Models;

using System.Text.Json.Serialization;

public class OpenAIChatCompletionsChoice
{
    [JsonPropertyName("index")]
    public int Index { get; init; }

    [JsonPropertyName("delta")]
    public OpenAIChatCompletionsDelta Delta { get; init; } = new OpenAIChatCompletionsDelta();

    /// <summary>
    /// 可以为 null,表示未完成或没有特定的结束原因.<see cref="ChatFinishReason"/>."/>
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; init; }
}
