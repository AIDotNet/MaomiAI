using FastEndpoints;
using FluentValidation;
using MaomiAI.Document.Shared.Commands.Documents;

namespace MaomiAI.Document.Api.Validators;

public class PreUploadWikiDocumentCommandValidators : Validator<PreUploadWikiDocumentCommand>
{
    public PreUploadWikiDocumentCommandValidators()
    {
        RuleFor(x => x.WikiId)
            .NotEmpty()
            .WithMessage("知识库ID不能为空");
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("文件名称不能为空")
            .MaximumLength(100)
            .WithMessage("文件名称不能超过100个字符");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("文件类型不能为空")
            .MaximumLength(50)
            .WithMessage("文件类型不能超过50个字符");
    }
}
