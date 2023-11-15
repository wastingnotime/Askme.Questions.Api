using Askme.Questions.Api.Configuration;
using Askme.Questions.Api.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace Askme.Questions.Api.Repositories;

public class QuestionListRepository : IQuestionListRepository
{
    private static IEnumerable<QuestionListModel> _memoryStoreQList = Enumerable.Empty<QuestionListModel>();
    
    private readonly IMongoCollection<QuestionListModel> _collection;

    public QuestionListRepository(IMongoDbSettings settings) =>
        _collection = new MongoClient(settings.ConnectionString)
            .GetDatabase(settings.DatabaseName)
            .GetCollection<QuestionListModel>("questionList");

    /*
    if (!_started)
    {
        _memoryStoreQList = _memoryStoreQList.Append(new QuestionListModel
        {
            Id = "ac35cc7d-4aa8-43db-85f9-3af983b99999",
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
    */

    public Task<IEnumerable<QuestionListModel>> AllAsync()
    {
        return Task.FromResult(_collection.Find(new BsonDocument()).ToList().AsEnumerable<QuestionListModel>());
    }

    public Task<IEnumerable<QuestionListModel>> AllAsync(Expression<Func<QuestionListModel, bool>> expr)
    {
        return Task.FromResult(_collection.AsQueryable().Where(expr.Compile()));
    }

    public Task<QuestionListModel> OneAsync(Expression<Func<QuestionListModel, bool>> expr)
    {
        return _collection.AsQueryable().FirstOrDefaultAsync(expr);
    }

    public Task StoreAsync(QuestionListModel value)
    {
        var questionList = new QuestionListModel { Title = value.Title };
        _collection.InsertOne(questionList);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(QuestionListModel value)
    {
        return _collection.DeleteOneAsync(Builders<QuestionListModel>.Filter.Eq("Id", value.Id));
    }
}