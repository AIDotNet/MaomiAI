using MediatR;

namespace MaomiAI.AiModel.Shared.Models;

/// <summary>
/// AI 模型默认配置.
/// </summary>
public class AiModelDefaultConfiguration
{
    /// <summary>
    /// AI 服务商.
    /// </summary>
    public AiProvider Provider { get; init; }

    /// <summary>
    /// AI 模型的功能，判断是否多模态.
    /// </summary>
    public AiModelFunction Function { get; init; }

    /// <summary>
    /// 支持 function call.
    /// </summary>
    public bool IsSupportFunctionCall { get; init; }

    /// <summary>
    /// 支持图片.
    /// </summary>
    public bool IsSupportImg { get; init; }

    /// <summary>
    /// 请求端点.
    /// </summary>
    public string Enpoint { get; init; } = default!;
}