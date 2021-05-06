using System.ComponentModel.DataAnnotations;

namespace back.Data
{
    public class QuestionPutRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}