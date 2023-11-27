using Askme.Questions.Api.Model;
using Askme.Questions.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using System.Linq.Expressions;
using FluentValidation.TestHelper;

namespace Askme.Questions.Api.Controllers;

public class QuestionListControllerTest
{
    private ValidationQuestionList validator;

    [Fact]
    public async Task questionlist_noexistence()
    {
        var repositoryMock = new Mock<IQuestionListRepository>();
        repositoryMock
            .Setup(x => x.AllAsync()).ReturnsAsync(Enumerable.Empty<QuestionListModel>());

        var actual = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).GetQuestionListAsync();

        Assert.Empty(actual);
    }

    [Fact]
    public async Task questionlist_getall()
    {
        var repositoryMock = new Mock<IQuestionListRepository>();
        repositoryMock
            .Setup(x => x.AllAsync()).ReturnsAsync(Enumerable.Empty<QuestionListModel>());

        var actual = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).GetQuestionListAsync();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public async Task questionlist_getonenonexistence()
    {
        var repositoryMock = new Mock<IQuestionListRepository>();

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).GetQuestionListAsync(ObjectId.GenerateNewId().ToString());

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task questionlist_getoneexistence()
    {
        var expected = new QuestionListModel { Id = "655272a5ab12eddb6281de56" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
           .Setup(x => x.OneAsync(It.IsAny<Expression<Func<QuestionListModel, bool>>>()))
           .ReturnsAsync(expected);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).GetQuestionListAsync(expected.Id);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task questionlist_deleteitemnoexistence()
    {
        var expected = new QuestionListModel { Id = ObjectId.GenerateNewId().ToString()};

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
           .Setup(x => x.DeleteAsync(It.IsAny<QuestionListModel>()))
           .Returns(Task.CompletedTask);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).DeleteQuestionListAsync(expected.Id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task questionlist_deleteitemexistence()
    {
        var expected = new QuestionListModel { Id = "655272a5ab12eddb6281de56" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
            .Setup(x => x.OneAsync(It.IsAny<Expression<Func<QuestionListModel, bool>>>()))
            .ReturnsAsync(expected);

        repositoryMock
           .Setup(x => x.DeleteAsync(It.IsAny<QuestionListModel>()))
           .Returns(Task.CompletedTask);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).DeleteQuestionListAsync(expected.Id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task questionlist_create()
    {
        validator = new ValidationQuestionList();

        var expected = new QuestionListModel { Title = "Teste Título" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
            .Setup(x => x.StoreAsync(expected))
            .Returns(Task.CompletedTask);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object, validator).SaveQuestionListAsync(expected);

        Assert.IsType<CreatedAtActionResult>(result);
        var actual = ((CreatedAtActionResult)result).Value as QuestionListModel;
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Title, actual.Title);
    }

    private static ILogger<QuestionListsController> GetLoggerStub() => new Mock<ILogger<QuestionListsController>>().Object;

}