using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private static IEnumerable<AnswerModel> _memoryStoreAnswer = Enumerable.Empty<AnswerModel>();
    private static bool _started;

    public AnswerRepository()
    {
        if (!_started)
        {
            List<AnswerModel> Answers = new List<AnswerModel>();
            Answers.Add(new AnswerModel { Text = "Resposta 1-1", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 1-2", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 1-3", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 1-4", IsCorrect = true });
            Answers.Add(new AnswerModel { Text = "Resposta 1-5", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 2-1", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 2-2", IsCorrect = true });
            Answers.Add(new AnswerModel { Text = "Resposta 2-3", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 2-4", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 3-1", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 3-2", IsCorrect = true });
            Answers.Add(new AnswerModel { Text = "Resposta 3-3", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 3-4", IsCorrect = false });
            Answers.Add(new AnswerModel { Text = "Resposta 3-5", IsCorrect = false });

            _memoryStoreAnswer = Answers;

            _started = true;
        }
    }

    public Task<IEnumerable<AnswerModel>> AllAsync()
    {
        return Task.FromResult(_memoryStoreAnswer.AsEnumerable());
    }

    public Task<IEnumerable<AnswerModel>> AllAsync(Func<AnswerModel, bool> predicate)
    {
        return Task.FromResult(_memoryStoreAnswer.AsEnumerable().Where(predicate));
    }

    public Task<AnswerModel?> OneAsync(Func<AnswerModel, bool> predicate)
    {
        return Task.FromResult(_memoryStoreAnswer.AsEnumerable().FirstOrDefault(predicate));
    }

    public Task StoreAsync(AnswerModel contact)
    {
        if (_memoryStoreAnswer.AsEnumerable().Any(x => x.Id == contact.Id))
            return Task.CompletedTask;

        _memoryStoreAnswer = _memoryStoreAnswer.Append(contact);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AnswerModel contact)
    {
        _memoryStoreAnswer = _memoryStoreAnswer.Where(x => x.Id != contact.Id);
        return Task.CompletedTask;
    }
}