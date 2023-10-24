using Askme.Questions.Api.Model;

namespace Askme.Questions.Api.Repositories;

public class QuestionListRepository : IQuestionListRepository
{
    private static IEnumerable<QuestionListModel> _memoryStoreQList = Enumerable.Empty<QuestionListModel>();
    private static IEnumerable<QuestionModel> _memoryStoreQuestion = Enumerable.Empty<QuestionModel>();
    private static IEnumerable<AnswerModel> _memoryStoreAnswer = Enumerable.Empty<AnswerModel>();
    private static bool _started;

    public QuestionListRepository()
    {
        if (!_started)
        {
            _memoryStoreQList = _memoryStoreQList.Append(new QuestionListModel
            {
                Title = "Países",
                Questions = new[]
                {
                    new QuestionModel 
                    {   Title = "Qual o melhor pais com a letra A ?", 
                        Answers = new[] 
                        {
                            new AnswerModel { Text = "Afeganistão", IsCorrect = false },
                            new AnswerModel { Text = "África do Sul", IsCorrect = false },
                            new AnswerModel { Text = "Argentina", IsCorrect = false },
                            new AnswerModel { Text = "Alemanha", IsCorrect = true },
                            new AnswerModel { Text = "Angola", IsCorrect = false }
                        }                        
                    },
                    new QuestionModel 
                    {   Title = "Qual o melhor país para morar ?",
                        Answers = new[]
                        {
                            new AnswerModel { Text = "Alemanha", IsCorrect = false },
                            new AnswerModel { Text = "Brasil", IsCorrect = true },
                            new AnswerModel { Text = "Taiwan", IsCorrect = false },
                            new AnswerModel { Text = "Trindade e Tobago", IsCorrect = false }
                        }                    
                    },
                    new QuestionModel 
                    {   Title = "Qual o país mais visitado do mundo ?",
                        Answers = new[]
                        {
                            new AnswerModel { Text = "Espanha", IsCorrect = false },
                            new AnswerModel { Text = "França", IsCorrect = true },
                            new AnswerModel { Text = "Estados Unidos", IsCorrect = false },
                            new AnswerModel { Text = "Turquia", IsCorrect = false },
                            new AnswerModel { Text = "Itália", IsCorrect = false }
                        }
                    }
                }
            });

            _memoryStoreQList = _memoryStoreQList.Append(new QuestionListModel
            {
                Title = "Carros",
                Questions = new[]
                {
                    new QuestionModel { Title = "Qual o carro mais bonito ?" },
                    new QuestionModel { Title = "Qual o carro mais veloz ?" },
                    new QuestionModel { Title = "Qual o carro mais barato ?" },
                    new QuestionModel { Title = "Qual o carro mais caro ?" }
                }
            });

            _memoryStoreQList = _memoryStoreQList.Append(new QuestionListModel
            {
                Id = "7dd263e5-571f-4cf4-959b-0314d43a53e6",
                Title = "Atores Brasileiros",
                Questions = new[]
    {
                    new QuestionModel { 
                        Id = "59bd8599-18ef-46dc-bd6a-fe089078fb33",
                        Title = "Na sua opinião, qual a melhor atriz ?" ,
                        Answers = new[]
                        {
                            new AnswerModel { Text = "Alessandra Negrini", IsCorrect = false },
                            new AnswerModel { Id = "ac35cc7d-4aa8-43db-85f9-3af983b00827", Text = "Paola Oliveira", IsCorrect = true },
                        }
                    },
                    new QuestionModel { Id = "222f9dfa-fcb1-4c31-93e4-848c23b78964" , Title = "Qual ator/atriz mais velho atuando ?" }
                }
            });

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

    public Task StoreAsync(QuestionListModel value)
    {
        if (_memoryStoreQList.AsEnumerable().Any(x => x.Id == value.Id))
            return Task.CompletedTask;

        _memoryStoreQList = _memoryStoreQList.Append(value);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(QuestionListModel value)
    {
        _memoryStoreQList = _memoryStoreQList.Where(x => x.Id != value.Id);
        return Task.CompletedTask;
    }
}