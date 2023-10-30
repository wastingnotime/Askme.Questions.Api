using Askme.Questions.Api.Model;
using Askme.Questions.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Askme.Questions.Api.Controllers;

public class QuestionListControllerTest
{
    [Fact]
    public async Task questionlist_noexistence()
    {
        var repositoryMock = new Mock<IQuestionListRepository>();
        repositoryMock
            .Setup(x => x.AllAsync()).ReturnsAsync(Enumerable.Empty<QuestionListModel>());

        var actual = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).GetQuestionListAsync();

        Assert.Empty(actual);
    }

    [Fact]
    public async Task questionlist_getall()
    {
        var repositoryMock = new Mock<IQuestionListRepository>();
        repositoryMock
            .Setup(x => x.AllAsync()).ReturnsAsync(new[]
                {
                    new QuestionListModel { Title = "QuestionList 1" },
                    new QuestionListModel { Title = "QuestionList 2" },
                    new QuestionListModel { Title = "QuestionList 3" }
                });

        var actual = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).GetQuestionListAsync();

        Assert.NotNull(actual);
    }

    [Fact]
    public async Task questionlist_getonenonexistence()
    {
        var repositoryMock = new Mock<IQuestionListRepository>();

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).GetQuestionListAsync(Guid.NewGuid().ToString());

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task questionlist_getoneexistence()
    {
        var expected = new QuestionListModel { Id = Guid.NewGuid().ToString(), Title = "Países" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
           .Setup(x => x.OneAsync(It.IsAny<Func<QuestionListModel, bool>>()))
           .ReturnsAsync(expected);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).GetQuestionListAsync(expected.Id);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task questionlist_deleteitemnoexistence()
    {
        var expected = new QuestionListModel { Id = Guid.NewGuid().ToString(), Title = "Países" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
           .Setup(x => x.DeleteAsync(It.IsAny<QuestionListModel>()))
           .Returns(Task.CompletedTask);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).DeleteQuestionListAsync(expected.Id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task questionlist_deleteitemexistence()
    {
        var expected = new QuestionListModel { Id = Guid.NewGuid().ToString(), Title = "Países" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
            .Setup(x => x.OneAsync(It.IsAny<Func<QuestionListModel, bool>>()))
            .ReturnsAsync(expected);

        repositoryMock
           .Setup(x => x.DeleteAsync(It.IsAny<QuestionListModel>()))
           .Returns(Task.CompletedTask);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).DeleteQuestionListAsync(expected.Id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task questionlist_create()
    {
        var expected = new QuestionListModel { Id = Guid.NewGuid().ToString(), Title = "Países" };

        var repositoryMock = new Mock<IQuestionListRepository>();

        repositoryMock
            .Setup(x => x.StoreAsync(expected))
            .Returns(Task.CompletedTask);

        var result = await new QuestionListsController(GetLoggerStub(), repositoryMock.Object).SaveQuestionListAsync(expected);

        Assert.IsType<CreatedAtRouteResult>(result);
        var actual = ((CreatedAtRouteResult)result).Value as QuestionListModel;
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Title, actual.Title);
    }

    private static ILogger<QuestionListsController> GetLoggerStub() => new Mock<ILogger<QuestionListsController>>().Object;

}