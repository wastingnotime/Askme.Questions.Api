using FluentValidation;

namespace Askme.Questions.Api.Model;

public class ValidationQuestionList : AbstractValidator<QuestionListModel>
{
    public ValidationQuestionList()
    {
        RuleFor(x => x.Id).NotEqual("string");
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).NotEqual("string");
        RuleForEach(x => x.Questions).SetValidator(new ValidationQuestion());
    }
}
