namespace back.Data
{
    public interface IQuestionCache {
        void Set(QuestionGetSingleResponse question);
        void Remove(int questionId);
        QuestionGetSingleResponse Get(int questionId);
    }
}