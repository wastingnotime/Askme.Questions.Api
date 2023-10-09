using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public class QuestionListRepository : IQuestionListRepository
{
    private static IEnumerable<QuestionListModel> _memoryStoreQList = Enumerable.Empty<QuestionListModel>();
    private static bool _started;

    public QuestionListRepository()
    {
        if (!_started)
        {
            List<QuestionListModel> Qlist = new List<QuestionListModel>();
            Qlist.Add(new QuestionListModel { Title = "Lista Questão 1" });
            Qlist.Add(new QuestionListModel { Title = "Lista Questão 2" });
            Qlist.Add(new QuestionListModel { Title = "Lista Questão 3" });
            _memoryStoreQList = Qlist;

            _started = true;
        }
    }

    public Task<IEnumerable<QuestionListModel>> AllAsync()
    {
        return Task.FromResult(_memoryStoreQList.AsEnumerable());
    }

    public Task<IEnumerable<QuestionListModel>> AllAsync(Func<QuestionListModel, bool> predicate)
    {
        return Task.FromResult(_memoryStoreQList.AsEnumerable().Where(predicate));
    }

    public Task<QuestionListModel?> OneAsync(Func<QuestionListModel, bool> predicate)
    {
        return Task.FromResult(_memoryStoreQList.AsEnumerable().FirstOrDefault(predicate));
    }

    public Task StoreAsync(QuestionListModel contact)
    {
        if (_memoryStoreQList.AsEnumerable().Any(x => x.Id == contact.Id))
            return Task.CompletedTask;

        _memoryStoreQList = _memoryStoreQList.Append(contact);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(QuestionListModel contact)
    {
        _memoryStoreQList = _memoryStoreQList.Where(x => x.Id != contact.Id);
        return Task.CompletedTask;
    }
}