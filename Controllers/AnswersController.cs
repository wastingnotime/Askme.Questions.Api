
using Askme.Questions.Api.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Askme.Questions.Api.Controllers;

[ApiController]
[Route("[controller]/[Action]")]
public class AnswersController : ControllerBase
{
    private readonly ILogger<AnswersController> _logger;
    private static IEnumerable<AnswerModel> _memoryStore = Enumerable.Empty<AnswerModel>();

    public AnswersController(ILogger<AnswersController> logger)
    {
        _logger = logger;

        if (!_memoryStore.Any())
        {
            _memoryStore = _memoryStore.Append(new AnswerModel
            {
                Text = "Answer1",
                IsCorrect = true
            });

            _memoryStore = _memoryStore.Append(new AnswerModel
            {
                Text = "Answer2",
                IsCorrect = false
            });
        }
    }
/*
   
    private static AnswerModel? GetItem(string text) =>
        _memoryStore.FirstOrDefault(x => x.Text == text);
*/
}
