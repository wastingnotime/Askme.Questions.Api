namespace Askme.Questions.Api.Model;

public class QuestionModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }

    public IEnumerable<AnswerModel> Answers { get; set; } = Enumerable.Empty<AnswerModel>();

    public AnswerModel? GetAnswer(string idAnswer) => 
        Answers.FirstOrDefault(x => x.Id == idAnswer);

    public void DeleteAnswer(AnswerModel model)
    {
        var assistant = Answers.ToList();
        assistant.Remove(model);
        Answers = assistant;
    }

    public bool AddAnswer(AnswerModel model)
    {
        //if (this.Questions.Answer.Any(x => x.Title == model.Text))
        //    return false;

        Answers = Answers.Append(model);

        return true;
    }
}