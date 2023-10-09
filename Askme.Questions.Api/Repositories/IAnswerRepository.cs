using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public interface IAnswerRepository
{    
    public Task<IEnumerable<AnswerModel>> AllAsync();
    public Task<IEnumerable<AnswerModel>> AllAsync(Func<AnswerModel, bool> predicate);
    public Task<AnswerModel?> OneAsync(Func<AnswerModel, bool> predicate);
    public Task StoreAsync(AnswerModel answer);
    public Task DeleteAsync(AnswerModel answer);
}
