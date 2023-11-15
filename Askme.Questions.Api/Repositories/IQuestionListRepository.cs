using Askme.Questions.Api.Model;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Askme.Questions.Api.Repositories;

public interface IQuestionListRepository
{
    public Task<IEnumerable<QuestionListModel>> AllAsync();
    public Task<IEnumerable<QuestionListModel>> AllAsync(Expression<Func<QuestionListModel, bool>> expr);
    public Task<QuestionListModel?> OneAsync(Expression<Func<QuestionListModel, bool>> expr);
    public Task StoreAsync(QuestionListModel questionList);
    public Task DeleteAsync(QuestionListModel questionList);   
}
