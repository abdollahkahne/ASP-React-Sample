using System;
using System.ComponentModel.DataAnnotations;

namespace back.Data
{
    public class QuestionPostRequest
    {
        [Required]
        [StringLength(100,ErrorMessage="Maximum Length allowed is 100 chars")]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}