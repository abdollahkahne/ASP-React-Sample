using Microsoft.Extensions.Caching.Memory;

namespace back.Data
{
    public class QuestionCache:IQuestionCache {
        private MemoryCache _cache;

        public QuestionCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions{SizeLimit=100});
        }

        private string GetCacheKey(int questionId)=>$"Question-${questionId}";

        public QuestionGetSingleResponse Get(int questionId)
        {
            QuestionGetSingleResponse question;
            _cache.TryGetValue(GetCacheKey(questionId),out question);
            return question;
        }

        public void Remove(int questionId)
        {
            _cache.Remove(GetCacheKey(questionId));
        }

        public void Set(QuestionGetSingleResponse question)
        {
            var cacheEntryOption=new MemoryCacheEntryOptions{
                Size=1,
                AbsoluteExpirationRelativeToNow=new System.TimeSpan(1,0,0,0,0)
            };
            _cache.Set(GetCacheKey(question.QuestionId),question,cacheEntryOption);
        }
    }
}