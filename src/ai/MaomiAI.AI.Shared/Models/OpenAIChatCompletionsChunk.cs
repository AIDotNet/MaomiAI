using Microsoft.Extensions.AI;

namespace MaomiAI.AI.Models;

using System.Text.Json.Serialization;

public class OpenAIChatCompletionsChunk : OpenAIChatCompletionsObject
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
    /// 聊天完成选项列表。如果n大于1,可以有多个选项.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<OpenAIChatCompletionsChoice> Choices { get; init; } = new List<OpenAIChatCompletionsChoice>();

    /// <summary>
    /// 对象类型,总是 chat.completion.chunk.
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; } = "chat.completion.chunk"; // 默认值为 chat.completion.chunk

}
