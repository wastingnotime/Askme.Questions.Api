using FluentValidation;

namespace Askme.Questions.Api.Model;

public class ValidationQuestion : AbstractValidator<QuestionModel>
{
    public ValidationQuestion()
    {
        RuleFor(x => x.Id).NotEqual("string");
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).NotEqual("string");
        RuleForEach(x => x.Answers).SetValidator(new ValidationAnswer());
    }
}
