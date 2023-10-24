using Askme.Questions.Api.Model;
using Askme.Questions.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Askme.Questions.Api.Controllers;

public class QuestionListControllerTest
{
    [Fact]
    public async Task Cannot_get_all_questionlists_due_its_nonexistence()
    {
        var repositoryMockQL = new Mock<IQuestionListRepository>();
        repositoryMockQL
            .Setup(x => x.AllAsync()).ReturnsAsync(Enumerable.Empty<QuestionListModel>());

        //var repositoryMockQ = new Mock<IQuestionRepository>();
        //repositoryMockQ
        //    .Setup(x => x.AllAsync()).ReturnsAsync(Enumerable.Empty<QuestionModel>());

        //var repositoryMockA = new Mock<IAnswerRepository>();
        //repositoryMockA
        //    .Setup(x => x.AllAsync()).ReturnsAsync(Enumerable.Empty<AnswerModel>());

        var actual = await new QuestionListsController(GetLoggerStub(), repositoryMockQL.Object).GetQuestionListAsync();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public async Task Can_get_all_questionlists()
    {
        var repositoryMockQL = new Mock<IQuestionListRepository>();
        repositoryMockQL
            .Setup(x => x.AllAsync()).ReturnsAsync(new[]
                {
                    new QuestionListModel { Title = "Lista Questão 1" },
                    new QuestionListModel { Title = "Lista Questão 2" },
                    new QuestionListModel { Title = "Lista Questão 3" }
                });

        //var repositoryMockQ = new Mock<IQuestionRepository>();
        //repositoryMockQ
        //    .Setup(x => x.AllAsync()).ReturnsAsync(new[]
        //        {
        //            new QuestionModel { Title = "Questão 1-1" },
        //            new QuestionModel { Title = "Questão 1-2" },
        //            new QuestionModel { Title = "Questão 1-3" },
        //            new QuestionModel { Title = "Questão 2-1" },
        //            new QuestionModel { Title = "Questão 2-2" },
        //            new QuestionModel { Title = "Questão 2-3" },
        //            new QuestionModel { Title = "Questão 2-4" },
        //            new QuestionModel { Title = "Questão 3-1" }
        //        });

        //var repositoryMockA = new Mock<IAnswerRepository>();
        //repositoryMockA
        //    .Setup(x => x.AllAsync()).ReturnsAsync(new[]
        //        {
        //            new AnswerModel { Text = "Resposta 1-1", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 1-2", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 1-3", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 1-4", IsCorrect = true  },
        //            new AnswerModel { Text = "Resposta 1-5", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 2-1", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 2-2", IsCorrect = true  },
        //            new AnswerModel { Text = "Resposta 2-3", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 2-4", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 3-1", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 3-2", IsCorrect = true  },
        //            new AnswerModel { Text = "Resposta 3-3", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 3-4", IsCorrect = false },
        //            new AnswerModel { Text = "Resposta 3-5", IsCorrect = false }
    //});

        var actual = await new QuestionListsController(GetLoggerStub(), repositoryMockQL.Object).GetQuestionListAsync();

        Assert.NotNull(actual);
        Assert.Equal(2, actual.Count());
        Assert.Equal("Resposta 1-1", actual.First().Title);
        Assert.Equal("Resposta 1-4", actual.Skip(1).First().Title);
    }

    [Fact]
    public async Task Cannot_get_one_questionlists_due_its_nonexistence()
    {
        var repositoryMockQL = new Mock<IQuestionListRepository>();

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMockQL.Object).GetQuestionListAsync();

        Assert.IsType<NotFoundResult>(result);
    }


/*

    public async Task Can_get_one_questionlists()
    public async Task Cannot_delete_a_questionlists_due_its_nonexistence()
    public async Task Can_delete_a_questionlists()
    public async Task Can_create_a_questionlists()
    public async Task Can_update_a_questionlists()
*/

    private static ILogger<QuestionListsController> GetLoggerStub() => new Mock<ILogger<QuestionListsController>>().Object;

}