namespace Askme.Questions.Api.Model;

public class AnswerModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string? Text { get; set; }

    public bool IsCorrect { get; set; }
}