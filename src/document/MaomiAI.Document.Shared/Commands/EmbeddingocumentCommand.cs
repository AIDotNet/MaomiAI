using MediatR;

namespace MaomiAI.Document.Core.Handlers;

/// <summary>
/// 执行对文件的处理.
/// </summary>
public class EmbeddingocumentCommand : IRequest<EmptyCommandResponse>
{
    public Guid TeamId { get; init; }
    public Guid WikiId { get; init; }
    public Guid DocumentId { get; init; }

    /// <summary>
    /// The maximum number of tokens per paragraph.
    /// When partitioning a document, each partition usually contains one paragraph.
    /// </summary>
    public int MaxTokensPerParagraph { get; set; } = 1000;

    /// <summary>
    /// The number of overlapping tokens between chunks.
    /// </summary>
    public int OverlappingTokens { get; set; } = 100;

    /// <summary>
    /// Name of the tokenizer used to count tokens.
    /// Supported values: "p50k", "cl100k", "o200k". Leave it empty for autodetect.
    /// </summary>
    public string Tokenizer { get; set; } = string.Empty;
}

public class SetEmbeddingGenerationDocumentTaskCommand : IRequest<EmptyCommandResponse>
{
    public Guid WikiId { get; init; }
    public Guid DocumentId { get; init; }
}
