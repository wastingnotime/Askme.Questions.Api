namespace Askme.Questions.Api.Model;

public class QuestionListModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }

    public IEnumerable<QuestionModel> Questions { get; set; } = Enumerable.Empty<QuestionModel>();

    public bool AddQuestion(QuestionModel model)
    {
        if (this.Questions.Any(x => x.Title == model.Title))
            return false;

        this.Questions = this.Questions.Append(model);

        return true;
    }

    public QuestionModel GetQuestion(string idQuestion)
    {
        return this.Questions.Where(x => x.Id == idQuestion).FirstOrDefault();
    }
}