using System.Collections.Generic;
using back.Data;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController:ControllerBase {
        private readonly IDataRepository _dataRepository;

        public QuestionsController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public IEnumerable<QuestionGetManyResponses> GetQuestions(string search) {
            if (string.IsNullOrEmpty(search)) {
                return _dataRepository.GetQuestions();
            }
            var questions=_dataRepository.GetQuestionBySearch(search);
           
            return questions;
        }
        [HttpGet("unanswered")]
        public IEnumerable<QuestionGetManyResponses> GetUnAnsweredQuestions() {
            return _dataRepository.GetUnAnsweredQuestions();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetQuestion(int questionId) {
            var question=_dataRepository.GetQuestion(questionId);
            if (question!=null)
            return Ok(question); // We can use return question directly too
            else 
            return NotFound();
        }

        [HttpPost]
        public ActionResult<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest question) {
            var addedQuestion=_dataRepository.PostQuestion(question);
            return CreatedAtAction(nameof(GetQuestion),new {questionId=addedQuestion.QuestionId},addedQuestion);
        }
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> UpdateQuestion(int questionId,QuestionPutRequest question) {
            if (_dataRepository.QuestionExists(questionId))
                return _dataRepository.PutQuestion(questionId,question);
            else 
                return NotFound();
        }
        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId) {
            if (_dataRepository.QuestionExists(questionId)) {
                _dataRepository.DeleteQuestion(questionId);
                return NoContent();
            }
            else 
                return NotFound();
        }
        [HttpPost("answer")]
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostRequest answer) {
            if (_dataRepository.QuestionExists(answer.QuestionId)) {
                var addedAnswer=_dataRepository.PostAnswer(answer);
                return addedAnswer;
            } else {
                return NotFound();
            }
        }
    }
}