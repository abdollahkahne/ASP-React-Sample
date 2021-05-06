using System;

namespace back.Data
{
    public class Question
    {
        public string Title { get; set;}
        public string Content { get; set; }
        public string UserId {get; set;  }
        public string UserName {get; set;  }
        public DateTime Created { get; set; }
    }
}