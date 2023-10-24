namespace Askme.Questions.Api.Model;

public class QuestionModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }

    public IEnumerable<AnswerModel> Answers { get; set; } = Enumerable.Empty<AnswerModel>();

    public AnswerModel GetAnswer(string idAnswer)
    {
        return this.Answers.Where(x => x.Id == idAnswer).FirstOrDefault();
    }

    public bool AddAnswer(AnswerModel model)
    {
        //if (this.Questions.Answer.Any(x => x.Title == model.Text))
        //    return false;

        //this.Questions = this.Questions.Answer.Append(model);

        return true;
    }
}