namespace Askme.Questions.Api.Model;

public class QuestionListModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }
}