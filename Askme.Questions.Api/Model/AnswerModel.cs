using MongoDB.Bson;

namespace Askme.Questions.Api.Model;

public class AnswerModel
{
    public ObjectId Id { get; set; }

    public string? Text { get; set; }

    public bool IsCorrect { get; set; }
}