using System;
using System.ComponentModel.DataAnnotations;

namespace back.Data
{
    public class AnswerPostRequest
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}