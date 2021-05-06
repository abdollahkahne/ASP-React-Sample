using System.Collections.Generic;

namespace back.Data
{
    public interface IDataRepository {
        IEnumerable<QuestionGetManyResponses> GetQuestions();
        IEnumerable<QuestionGetManyResponses> GetQuestionsWithAnswers();
        IEnumerable<QuestionGetManyResponses> GetQuestionsWithAnswerUsingJoin();
        IEnumerable<QuestionGetManyResponses> GetQuestionBySearch(string search);
        IEnumerable<QuestionGetManyResponses> GetUnAnsweredQuestions();
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
        QuestionGetSingleResponse PostQuestion(Question question);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question);
        void DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(Answer answer);
    }
}