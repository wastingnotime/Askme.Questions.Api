using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Askme.Questions.Api.Model;

public class QuestionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Title { get; set; }

    public IEnumerable<AnswerModel> Answers { get; set; } = Enumerable.Empty<AnswerModel>();

    public AnswerModel? GetAnswer(ObjectId idAnswer) => 
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