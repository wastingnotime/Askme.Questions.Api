using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public interface IQuestionListRepository
{
    //QuestionList
    public Task<IEnumerable<QuestionListModel>> AllAsync();
    public Task<IEnumerable<QuestionListModel>> AllAsync(Func<QuestionListModel, bool> predicate);
    public Task<QuestionListModel?> OneAsync(Func<QuestionListModel, bool> predicate);
    public Task StoreAsync(QuestionListModel questionList);
    public Task DeleteAsync(QuestionListModel questionList);

    /*
    //Question
    public Task StoreQuestionAsync(QuestionModel question);
    public Task<IEnumerable<QuestionModel>> AllQuestionAsync(string idQuestionList);
    public Task<IEnumerable<QuestionModel>> AllQuestionAsync(Func<QuestionModel, bool> predicate);
    public Task<QuestionModel?> OneQuestionAsync(Func<QuestionModel, bool> predicate);
    public Task DeleteQuestionAsync(QuestionModel question);


    //Answer
    public Task<IEnumerable<AnswerModel>> AllAnswerAsync();
    public Task<IEnumerable<AnswerModel>> AllAnswerAsync(Func<AnswerModel, bool> predicate);
    public Task<AnswerModel?> OneAnswerAsync(Func<AnswerModel, bool> predicate);
    public Task StoreAnswerAsync(AnswerModel answer);
    public Task DeleteAnswerAsync(AnswerModel answer);
    */
}
