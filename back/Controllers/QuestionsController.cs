using System;
using System.Collections.Generic;
using back.Data;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;

        public QuestionsController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public IEnumerable<QuestionGetManyResponses> GetQuestions(string search, bool includeAnswers)
        {
            if (string.IsNullOrEmpty(search))
            {
                if (includeAnswers)
                { return _dataRepository.GetQuestionsWithAnswerUsingJoin(); }
                else
                { return _dataRepository.GetQuestions(); }
            }
            var questions = _dataRepository.GetQuestionBySearch(search);

            return questions;
        }
        [HttpGet("unanswered")]
        public IEnumerable<QuestionGetManyResponses> GetUnAnsweredQuestions()
        {
            return _dataRepository.GetUnAnsweredQuestions();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetQuestion(int questionId)
        {
            var question = _dataRepository.GetQuestion(questionId);
            if (question != null)
                return Ok(question); // We can use return question directly too
            else
                return NotFound();
        }

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
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> UpdateQuestion(int questionId, QuestionPutRequest question)
        {
            if (_dataRepository.QuestionExists(questionId))
                return _dataRepository.PutQuestion(questionId, question);
            else
                return NotFound();
        }
        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId)
        {
            if (_dataRepository.QuestionExists(questionId))
            {
                _dataRepository.DeleteQuestion(questionId);
                return NoContent();
            }
            else
                return NotFound();
        }
        [HttpPost("answer")]
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostRequest answer)
        {
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