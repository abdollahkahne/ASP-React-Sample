using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using back.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IQuestionCache _cache;

        public QuestionsController(IDataRepository dataRepository, IQuestionCache cache)
        {
            _dataRepository = dataRepository;
            _cache = cache;
        }

        [HttpGet]
        public IEnumerable<QuestionGetManyResponses> GetQuestions(string search, bool includeAnswers,int page=1,int pageSize=20)
        {
            if (string.IsNullOrEmpty(search))
            {
                if (includeAnswers)
                { return _dataRepository.GetQuestionsWithAnswerUsingJoin(); }
                else
                { return _dataRepository.GetQuestions(); }
            }
            var questions = _dataRepository.GetQuestionsBySearchWithPagination(search,page,pageSize);

            return questions;
        }
        [HttpGet("unanswered")]
        public async Task<IEnumerable<QuestionGetManyResponses>> GetUnAnsweredQuestions()
        {
            return await _dataRepository.GetUnAnsweredQuestionsAsync();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetQuestion(int questionId)
        {
            var question=_cache.Get(questionId);
            if (question==null) {
                question = _dataRepository.GetQuestion(questionId);
                if (question != null)
                    _cache.Set(question);
                else
                    return NotFound();
            }
            return Ok(question); // Read it from Memory Cache if already exist there
                
        }

        [Authorize]
        [HttpPost]
        public ActionResult<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest question)
        {
            var addedQuestion = _dataRepository.PostQuestion(new Question
            {
                Title = question.Title,
                Content = question.Content,
                UserId = "1",
                UserName = "Bob.test@test.com",
                Created = DateTime.UtcNow,
            });
            return CreatedAtAction(nameof(GetQuestion), new { questionId = addedQuestion.QuestionId }, addedQuestion);
        }

        [Authorize(Policy ="QuestionAuthor")]
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> UpdateQuestion(int questionId, QuestionPutRequest question)
        {
            _cache.Remove(questionId);
            if (_dataRepository.QuestionExists(questionId))
                return _dataRepository.PutQuestion(questionId, question);
            else
                return NotFound();
        }

        [Authorize]
        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId)
        {
            _cache.Remove(questionId);
            if (_dataRepository.QuestionExists(questionId))
            {
                _dataRepository.DeleteQuestion(questionId);
                return NoContent();
            }
            else
                return NotFound();
        }

        [Authorize]
        [HttpPost("answer")]
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostRequest answer)
        {
            _cache.Remove(answer.QuestionId);
            if (_dataRepository.QuestionExists(answer.QuestionId))
            {
                var addedAnswer = _dataRepository.PostAnswer(new Answer
                {
                    QuestionId = answer.QuestionId,
                    Content = answer.Content,
                    UserId = "1",
                    UserName = "Bob.test@test.com",
                    Created = DateTime.UtcNow
                });
                return addedAnswer;
            }
            else
            {
                return NotFound();
            }
        }
    }
}