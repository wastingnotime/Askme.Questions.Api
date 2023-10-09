using Askme.Questions.Api.Model;
using Askme.Questions.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Askme.Questions.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionListsController : ControllerBase
{
    private readonly ILogger<QuestionListsController> _logger;
    private readonly IQuestionListRepository _repositoryQL;
    private readonly IQuestionRepository _repositoryQ;
    private readonly IAnswerRepository _repositoryA;

    public QuestionListsController(ILogger<QuestionListsController> logger, IQuestionListRepository repositoryQL,
                                   IQuestionRepository repositoryQ, IAnswerRepository repositoryA)
    {
        _logger = logger;
        _repositoryQL = repositoryQL;
        _repositoryQ = repositoryQ;
        _repositoryA= repositoryA;
    }

    #region QuestList

    /// <summary>
    /// Método para retornar a Lista de Questões
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<QuestionListModel>> GetAsync() => _repositoryQL.AllAsync();

    /// <summary>
    /// Método para salvar a Lista de Questões
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveQuestion(QuestionListModel value)
    {
        await _repositoryQL.StoreAsync(value);
        return CreatedAtAction(nameof(GetQuestionAsync), new { idQuestionList = value.Id }, value);
        //return CreatedAtAction("PostAsync", value);
    }

    /// <summary>
    /// Método para obter um item da Lista de Questões
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionListModel>> GetQuestionAsync(string idQuestionList)
    {
        var item = await _repositoryQL.OneAsync(x => x.Id == idQuestionList);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Método para excluir um item da Lista de Questões
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{idQuestionList:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestion(string idQuestionList)
    {
        var item = await _repositoryQL.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        await _repositoryQL.DeleteAsync(item);

        return NoContent();
    }

    #endregion

    #region Question

    [HttpGet("{idQuestionList:length(36)}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<QuestionModel>> GetAsyncQuestion(string idQuestionList) => _repositoryQ.AllAsync();

    /// <summary>
    /// Método para salvar uma nova questão
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("{idQuestionList:length(36)}/questions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveQuestion(string idQuestionList, QuestionModel value)
    {
        await _repositoryQ.StoreAsync(value);
        return CreatedAtAction(nameof(GetAsyncQuestion), new { id = value.Id }, value);
    }

    /// <summary>
    /// Método para obter um item referente a uma lista de questões
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/question/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<QuestionModel>> GetQuestion(string idQuestionList, string idQuestion)
    {
        var item = GetAsyncQuestion(idQuestionList);
        return Task.FromResult((ActionResult<QuestionModel>)(item is null ? NotFound() : Ok(item)));
    }

    /// <summary>
    /// Método para excluir uma questão
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <returns></returns>
    [HttpDelete("{idQuestionList:length(36)}/question/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestion(string idQuestionList, string idQuestion)
    {
        var item = await _repositoryQ.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        await _repositoryQ.DeleteAsync(item);

        return NoContent();
    }

    /// <summary>
    /// Método para alterar um questão
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPut("{idQuestionList:length(36)}/question/{idQuestion:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuestion(string idQuestionList, string idQuestion, QuestionModel value)
    {
        if (!idQuestionList.Equals(value.Id))
            return ValidationProblem(); //ValidationProblem instead BadRequest to keep standard 

        var item = await _repositoryQ.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        //TODO: repo? responsibility
        item.Id = value.Id;
        item.Title = value.Title;

        await _repositoryQ.StoreAsync(item);

        return NoContent();
    }

    #endregion

    #region Answer

    [HttpGet("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<AnswerModel>> GetAsyncAnswer(string idQuestionList, string idQuestion) => _repositoryA.AllAsync();

    /// <summary>
    /// Método para salvar uma nova pergunta
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveAswer(string idQuestionList, string idQuestion, AnswerModel value)
    {
        await _repositoryA.StoreAsync(value);
        return CreatedAtAction(nameof(GetAsyncQuestion), new { id = value.Id }, value);
    }

    /// <summary>
    /// Método para excluir uma pergunta
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
        var item = await _repositoryA.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        await _repositoryA.DeleteAsync(item);

        return NoContent();
    }

    /// <summary>
    /// Método para altarar uma pergunta
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="idAnswer"></param>
    /// <returns></returns>
    [HttpPut("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers/{idAnswer:length(36)}")] //Alterar para 36
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAnswer(string idQuestionList, string idQuestion, string idAnswer, AnswerModel value)
    {        
        if (!idQuestionList.Equals(value.Id))
            return ValidationProblem(); //ValidationProblem instead BadRequest to keep standard 

        var item = await _repositoryQ.OneAsync(x => x.Id == idQuestionList);
        if (item is null)
            return NotFound();

        //TODO: repo? responsibility
        item.Id = value.Id;
        item.Title = value.Text;

        await _repositoryQ.StoreAsync(item);

        return NoContent();        
    }

    /// <summary>
    /// Método para obter uma pergunta
    /// </summary>
    /// <param name="idQuestionList"></param>
    /// <param name="idQuestion"></param>
    /// <param name="idAnswer"></param>
    /// <returns></returns>
    [HttpGet("{idQuestionList:length(36)}/questions/{idQuestion:length(36)}/answers/{idAnswer:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<AnswerModel>> GetAnswer(string idQuestionList, string idQuestion, string idAnswer)
    {
        var item = GetAsyncAnswer(idQuestionList, idQuestion);
        return Task.FromResult((ActionResult<AnswerModel>)(item is null ? NotFound() : Ok(item)));
    }

    #endregion

}
