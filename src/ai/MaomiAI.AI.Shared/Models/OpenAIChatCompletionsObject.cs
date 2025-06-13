namespace MaomiAI.AI.Models;

using System.Text.Json.Serialization;

public interface OpenAIChatCompletionsObject
{
    /// <summary>
    /// 聊天完成的唯一标识符
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// 创建聊天完成的Unix时间戳(秒)
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; }

    /// <summary>
    /// 用于聊天完成的模型
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; }

    /// <summary>
    /// 该指纹表示模型运行的后端配置
    /// </summary>
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; }

    /// <summary>
    /// 对象类型,总是 chat.completion
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; }
}
