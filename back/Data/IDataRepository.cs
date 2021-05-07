using System.Collections.Generic;
using System.Threading.Tasks;

namespace back.Data
{
    public interface IDataRepository {
        IEnumerable<QuestionGetManyResponses> GetQuestions();
        IEnumerable<QuestionGetManyResponses> GetQuestionsWithAnswers();
        IEnumerable<QuestionGetManyResponses> GetQuestionsWithAnswerUsingJoin();
        IEnumerable<QuestionGetManyResponses> GetQuestionBySearch(string search);
        IEnumerable<QuestionGetManyResponses> GetQuestionsBySearchWithPagination(string search, int page, int pageSize);
        IEnumerable<QuestionGetManyResponses> GetUnAnsweredQuestions();
        Task<IEnumerable<QuestionGetManyResponses>> GetUnAnsweredQuestionsAsync();
        QuestionGetSingleResponse GetQuestion(int questionId);
        QuestionGetSingleResponse GetQuestionWithMulti(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
        QuestionGetSingleResponse PostQuestion(Question question);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question);
        void DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(Answer answer);
    }
}