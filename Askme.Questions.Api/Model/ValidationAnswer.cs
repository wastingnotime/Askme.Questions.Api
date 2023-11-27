using FluentValidation;

namespace Askme.Questions.Api.Model;

public class ValidationAnswer : AbstractValidator<AnswerModel>
{
    public ValidationAnswer()
    {
        RuleFor(x => x.Id).NotEqual("string");
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.Text).NotEqual("string");         
    }
}
