namespace ProSeeker.Data.Models.Quiz
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Answer : BaseDeletableModel<string>
    {
        public Answer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(150)]
        public string Text { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
