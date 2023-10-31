using Askme.Questions.Api.Model;
using Askme.Questions.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Askme.Questions.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionListsController : ControllerBase
{
    private readonly ILogger<QuestionListsController> _logger;
    private readonly IQuestionListRepository _repository;

    public QuestionListsController(ILogger<QuestionListsController> logger, IQuestionListRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    #region QuestList

    /// <summary>
    /// Method used to return QuestionList
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<QuestionListModel>> GetQuestionListAsync() => _repository.AllAsync();

    /// <summary>
    /// Method used to get QuestionList
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}")]
    //[Route(template: "{idQuestionList:length(36)}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionListModel>> GetQuestionListAsync(string idQuestionList)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Method used to save a new QuestionList
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost]
    //[Route(template: "{idQuestionList:length(36)}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveQuestionListAsync(QuestionListModel value)
    {
        await _repository.StoreAsync(value);
        return CreatedAtAction(nameof(GetQuestionListAsync), new { idQuestionList = value.Id }, value);
    }

    /// <summary>
    /// Method used to delete QuestionList
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <returns></returns>
    [HttpDelete("{idQuestionList:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestionListAsync(string idQuestionList)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        await _repository.DeleteAsync(item);

        return NoContent();
    }

    #endregion

    #region Question

    private IEnumerable<QuestionModel> GetQuestions(string idQuestionList)
    {
        var item = _repository.OneAsync(x => x.Id == idQuestionList);
        return item.Result.Questions;
    }

    /// <summary>
    /// Method used to get some Questions
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult> GetQuestionAsync(string idQuestionList)
    {
        var item = GetQuestions(idQuestionList);
        return Task.FromResult((ActionResult)(!item.Any() ? NotFound() : Ok(item)));
    }

    /// <summary>
    /// Method used to get a Question
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/question/{idQuestion:length(36)}")]
    //[Route(template: "{idQuestionList:length(36)}/question/{idQuestion:length(36)}", Name = "GetQ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult> GetQuestionAsync(string idQuestionList, string idQuestion)
    {
        var questions = GetQuestions(idQuestionList);
        var item = questions.Where(x => x.Id == idQuestion);

        return Task.FromResult((ActionResult)(!item.Any() ? NotFound() : Ok(item)));
    }

    /// <summary>
    /// Method used to save a new Question
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("{idQuestionList:length(36)}/questions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveQuestionAsync(string idQuestionList, QuestionModel value)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);

        if (item is null)
            return NotFound();

        if (!item.AddQuestion(value))
            return BadRequest("Erro ao inserir uma nova questão.");

        await _repository.StoreAsync(item);

        return CreatedAtAction(nameof(GetQuestionAsync), new { idQuestionList, idQuestion = value.Id }, value);
    }

    /// <summary>
    /// Method used to delete a Question
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpDelete("{idQuestionList:length(36)}/question/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestionAsync(string idQuestionList, string idQuestion)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);

        if (item is null)
            return NotFound();

        var question = item.GetQuestion(idQuestion);

        if (question is not null)
        {
            var assistant = item.Questions.ToList();
            assistant.Remove(question);
            item.Questions = assistant;

            await _repository.StoreAsync(item);
        }

        return NoContent();
    }

    /// <summary>
    /// Method used to update a Question
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPut("{idQuestionList:length(36)}/question/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuestionAsync(string idQuestionList, string idQuestion, QuestionModel value)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);

        if (item is null)
            return NotFound();

        var question = item.GetQuestion(idQuestion);

        if (question is null)
            return NotFound();

        question.Title = value.Title;
        await _repository.StoreAsync(item);

        return NoContent();
    }

    #endregion

    #region Answer
    
    private IEnumerable<AnswerModel> GetAnswers(string idQuestionList, string idQuestion)
    {
        var questions = GetQuestions(idQuestionList);
        QuestionModel? item = null;
    
        if (questions.Any())
            item = questions.Where(a => a.Id == idQuestion).FirstOrDefault();
    
        return item.Answers;
    }
    
    /// <summary>
    /// Method used to get some Answers
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult> GetAnswersAsync(string idQuestionList, string idQuestion)
    {
        var answers = GetAnswers(idQuestionList, idQuestion);
        return Task.FromResult((ActionResult)(!answers.Any() ? NotFound() : Ok(answers)));
    }
    
    /// <summary>
    /// Method used to get an Answer
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="idAnswer"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers/{idAnswer:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult> GetAnswerAsync(string idQuestionList, string idQuestion, string idAnswer)
    {
        var answers = GetAnswers(idQuestionList, idQuestion);
        var item = answers.Where(x => x.Id == idAnswer);
    
        return Task.FromResult((ActionResult)(item is null ? NotFound() : Ok(item)));
    }
    
    /// <summary>
    /// Method used to save a new Answer
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveAswerAsync(string idQuestionList, string idQuestion, AnswerModel value)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);
    
        if (item is null)
            return NotFound();
    
        if (item.Questions is not null)
        {
            var question = item.GetQuestion(idQuestion);
    
            if (question is null)
                return NotFound();
    
            question.Answers = question.Answers.Append(value);
            await _repository.StoreAsync(item);
        }
    
        return CreatedAtAction(nameof(GetAnswerAsync),  new { idQuestionList, idQuestion , idAnswer = value.Id}, value);
    }
    
    /// <summary>
    /// Method used to delete an Answer
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="idAnswer"></param>
    /// <returns></returns>
    [HttpDelete("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers/{idAnswer:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAnswer(string idQuestionList, string idQuestion, string idAnswer)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);        
    
        if (questionList is null)
            return NotFound();
    
        var questions = questionList.Questions;
        var question = questionList.GetQuestion(idQuestion);
    
        if (question is null)
            return NotFound();
    
        var answer = question.GetAnswer(idAnswer);
    
        if (answer is null)
            return NotFound();
    
        //TODO testar com mais respostas
        var assistant = question.Answers.ToList();
        assistant.Remove(answer);
        question.Answers = assistant;
        questionList.Questions = questions;
    
        await _repository.StoreAsync(questionList);
    
        return NoContent();
    }
    
    /// <summary>
    /// Method used to update an Answer
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="idAnswer"></param>
    /// <returns></returns>
    [HttpPut("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers/{idAnswer:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAnswer(string idQuestionList, string idQuestion, string idAnswer, AnswerModel value)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
    
        if (questionList is null)
            return NotFound();
    
        var question = questionList.GetQuestion(idQuestion);
    
        if (question is null)
            return NotFound();
    
        var answer = question.GetAnswer(idAnswer);
    
        if (answer is null)
            return NotFound();
    
        answer.Text = value.Text;
        answer.IsCorrect = value.IsCorrect;
    
        await _repository.StoreAsync(questionList);
    
        return NoContent();
    }
    
    #endregion
}