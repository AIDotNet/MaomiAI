using FastEndpoints;
using FluentValidation;
using MaomiAI.Document.Shared.Commands;

namespace MaomiAI.Document.Api.Validators;

public class CreateWikiCommandValidators : Validator<CreateWikiCommand>
{
    public CreateWikiCommandValidators()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队ID不能为空");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("知识库名称不能为空")
            .MaximumLength(50)
            .WithMessage("知识库名称不能超过50个字符");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("知识库描述不能为空")
            .MaximumLength(200)
            .WithMessage("知识库描述不能超过200个字符");
    }
}
