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
    public Task<IEnumerable<QuestionListModel>> GetQuestionListAsync() 
        => _repository.AllAsync();

    /// <summary>
    /// Method used to get QuestionList
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionListModel>> GetQuestionListAsync(string idQuestionList)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        return questionList is null ? NotFound() : Ok(questionList);
    }

    /// <summary>
    /// Method used to save a new QuestionList
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost]
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
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (questionList is null)
            return NotFound();

        await _repository.DeleteAsync(questionList);

        return NoContent();
    }

    #endregion

    #region Question

    /// <summary>
    /// Method used to get some Questions
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetQuestionAsync(string idQuestionList)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        return questionList is null ? NotFound() : Ok(questionList.Questions);
    }

    /// <summary>
    /// Method used to get a Question
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQuestionAsync(string idQuestionList, string idQuestion)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (questionList is null)
            return NotFound();

        var question = questionList.GetQuestion(idQuestion);

        return question is null ? NotFound() : Ok(question);
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
    [HttpDelete("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestionAsync(string idQuestionList, string idQuestion)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (questionList is null)
            return NotFound();

        var question = questionList.GetQuestion(idQuestion);
        if (question is null)
            return NotFound();

        questionList.RemoveQuestion(question);

        await _repository.StoreAsync(questionList);

        return NoContent();
    }

    /// <summary>
    /// Method used to update a Question
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPut("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuestionAsync(string idQuestionList, string idQuestion, QuestionModel value)
    {
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (questionList is null)
            return NotFound();

        var question = questionList.GetQuestion(idQuestion);
        if (question is null)
            return NotFound();

        question.Title = value.Title;

        await _repository.StoreAsync(questionList);

        return NoContent();
    }

    #endregion

    #region Answer

    /// <summary>
    /// Method used to get some Answers
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAnswersAsync(string idQuestionList, string idQuestion)
    {
        var questionListModel = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (questionListModel is null)
            return NotFound();

        var question = questionListModel.GetQuestion(idQuestion);
        if (question is null)
            return NotFound();

        return Ok(question.Answers);
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
    public async Task<ActionResult> GetAnswerAsync(string idQuestionList, string idQuestion, string idAnswer)
    {
        var item = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        var question = item.GetQuestion(idQuestion);
        if (question is null)
            return NotFound();

        var answer = question.GetAnswer(idAnswer);
        if (answer is null)
            return NotFound();

        return Ok(item);
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
        var questionList = await _repository.OneAsync(x => x.Id == idQuestionList);
        if (questionList is null)
            return NotFound();

        var question = questionList.GetQuestion(idQuestion);
        if (question is null)
            return NotFound();

        question.AddAnswer(value);

        await _repository.StoreAsync(questionList);

        return CreatedAtAction(nameof(GetAnswerAsync), new { idQuestionList, idQuestion, idAnswer = value.Id }, value);
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

        var question = questionList.GetQuestion(idQuestion);
        if (question is null)
            return NotFound();

        var answer = question.GetAnswer(idAnswer);
        if (answer is null)
            return NotFound();

        //TODO testar com mais respostas
        question.DeleteAnswer(answer);

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
    public async Task<IActionResult> UpdateAnswer(string idQuestionList, string idQuestion, string idAnswer,
        AnswerModel value)
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