namespace ProSeeker.Data.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Answers = new HashSet<Answer>();
        }

        [Required]
        [MaxLength(150)]
        public string Text { get; set; }

        public int Number { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public string SurveyId { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
