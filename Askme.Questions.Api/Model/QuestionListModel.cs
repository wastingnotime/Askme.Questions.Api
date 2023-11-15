using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Askme.Questions.Api.Model;

public class QuestionListModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Title { get; set; }

    public IEnumerable<QuestionModel> Questions { get; set; } = Enumerable.Empty<QuestionModel>();

    public override string ToString()
    {
        return $"{Id}: {Title}";
    }

    public bool AddQuestion(QuestionModel model)
    {
        if (this.Questions.Any(x => x.Title == model.Title))
            return false;

        this.Questions = this.Questions.Append(model);

        return true;
    }

    public QuestionModel? GetQuestion(string idQuestion)
    {
        return this.Questions.FirstOrDefault(x => x.Id == idQuestion);
    }

    public void RemoveQuestion(QuestionModel question)
    {
        var assistant = Questions.ToList();
        assistant.Remove(question);
        Questions = assistant;
    }

    public QuestionListModel Clone() =>
        (QuestionListModel)MemberwiseClone();
}