using System;
using System.ComponentModel.DataAnnotations;

namespace back.Data
{
    public class QuestionPostRequest
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}