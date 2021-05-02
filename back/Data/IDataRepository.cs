using System.Collections.Generic;

namespace back.Data
{
    public interface IDataRepository {
        IEnumerable<QuestionGetManyResponses> GetQuestions();
        IEnumerable<QuestionGetManyResponses> GetQuestionBySearch(string search);
        IEnumerable<QuestionGetManyResponses> GetUnAnsweredQuestions();
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
    }
}