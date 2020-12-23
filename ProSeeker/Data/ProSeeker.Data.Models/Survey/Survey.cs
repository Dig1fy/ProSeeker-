namespace ProSeeker.Data.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Survey : BaseDeletableModel<string>
    {
        public Survey()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Questions = new HashSet<Question>();
        }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
