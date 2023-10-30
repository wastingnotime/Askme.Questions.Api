using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public interface IQuestionListRepository
{
    public Task<IEnumerable<QuestionListModel>> AllAsync();
    public Task<IEnumerable<QuestionListModel>> AllAsync(Func<QuestionListModel, bool> predicate);
    public Task<QuestionListModel?> OneAsync(Func<QuestionListModel, bool> predicate);
    public Task StoreAsync(QuestionListModel questionList);
    public Task DeleteAsync(QuestionListModel questionList);
}
