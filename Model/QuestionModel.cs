namespace Askme.Questions.Api.Model;

public class QuestionModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }
}