using Microsoft.Extensions.AI;

namespace MaomiAI.AI.Models;

using System.Text.Json.Serialization;

public class OpenAIChatCompletionsUsage
{
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }
}
