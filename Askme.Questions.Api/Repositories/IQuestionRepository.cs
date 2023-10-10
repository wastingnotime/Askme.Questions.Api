using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public interface IQuestionRepository
{    
    public Task<IEnumerable<QuestionModel>> AllAsync(string idQuestionList);
    public Task<IEnumerable<QuestionModel>> AllAsync(Func<QuestionModel, bool> predicate);
    public Task<QuestionModel?> OneAsync(Func<QuestionModel, bool> predicate);
    public Task StoreAsync(QuestionModel question);
    public Task DeleteAsync(QuestionModel question);
}
