using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private static IEnumerable<QuestionModel> _memoryStoreQuestion = Enumerable.Empty<QuestionModel>();
    private static bool _started;

    public QuestionRepository()
    {
        if (!_started)
        {
            List<QuestionModel> Question = new List<QuestionModel>();
            Question.Add(new QuestionModel { Title = "Questão 1-1" });
            Question.Add(new QuestionModel { Title = "Questão 1-2" });
            Question.Add(new QuestionModel { Title = "Questão 1-3" });
            Question.Add(new QuestionModel { Title = "Questão 2-1" });
            Question.Add(new QuestionModel { Title = "Questão 2-2" });
            Question.Add(new QuestionModel { Title = "Questão 2-3" });
            Question.Add(new QuestionModel { Title = "Questão 2-4" });
            Question.Add(new QuestionModel { Title = "Questão 3-1" });
            _memoryStoreQuestion = Question;

            _started = true;
        }
    }

    public Task<IEnumerable<QuestionModel>> AllAsync(string idQuestionList)
    {
        return Task.FromResult(_memoryStoreQuestion.AsEnumerable().Where(x => x.Id == idQuestionList));
    }

    public Task<IEnumerable<QuestionModel>> AllAsync(Func<QuestionModel, bool> predicate)
    {
        return Task.FromResult(_memoryStoreQuestion.AsEnumerable().Where(predicate));
    }

    public Task<QuestionModel?> OneAsync(Func<QuestionModel, bool> predicate)
    {
        return Task.FromResult(_memoryStoreQuestion.AsEnumerable().FirstOrDefault(predicate));
    }

    public Task StoreAsync(QuestionModel contact)
    {
        if (_memoryStoreQuestion.AsEnumerable().Any(x => x.Id == contact.Id))
            return Task.CompletedTask;

        _memoryStoreQuestion = _memoryStoreQuestion.Append(contact);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(QuestionModel contact)
    {
        _memoryStoreQuestion = _memoryStoreQuestion.Where(x => x.Id != contact.Id);
        return Task.CompletedTask;
    }
}