using System;

namespace back.Data
{
    public class Answer
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}