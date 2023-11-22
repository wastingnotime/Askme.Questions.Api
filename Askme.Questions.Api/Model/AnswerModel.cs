using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Askme.Questions.Api.Model;

public class AnswerModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string? Text { get; set; }

    public bool IsCorrect { get; set; }

    public override string ToString()
    {
        return $"{Id}: {Text}: {IsCorrect}";
    }

}