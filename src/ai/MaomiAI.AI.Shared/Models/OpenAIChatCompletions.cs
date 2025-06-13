namespace MaomiAI.AI.Models;

using System.Text.Json.Serialization;

public class OpenAIChatCompletions : OpenAIChatCompletionsObject
{
    /// <summary>
    /// 聊天完成的唯一标识符
    /// </summary>
    [JsonPropertyName("id")]
    public virtual string Id { get; init; }

    /// <summary>
    /// 创建聊天完成的Unix时间戳(秒)
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; init; }

    /// <summary>
    /// 用于聊天完成的模型
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; init; }

    /// <summary>
    /// 该指纹表示模型运行的后端配置
    /// </summary>
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; init; }

    /// <summary>
    /// 聊天完成选项列表。如果n大于1,可以有多个选项
    /// </summary>
    [JsonPropertyName("choices")]
    public List<OpenAIChatCompletionsChoice> Choices { get; init; } = new List<OpenAIChatCompletionsChoice>();

    /// <summary>
    /// 完成请求的使用统计信息
    /// </summary>
    [JsonPropertyName("usage")]
    public OpenAIChatCompletionsUsage Usage { get; init; } = new OpenAIChatCompletionsUsage();

    /// <summary>
    /// 生成的完成中的标记数
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    /// <summary>
    /// 提示中的标记数
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    /// <summary>
    /// 请求中使用的标记总数(提示 + 完成)
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }

    /// <summary>
    /// 对象类型,总是 chat.completion.chunk.
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; init; } = "chat.completion"; // 默认值为 chat.completion.chunk
}
