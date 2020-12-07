namespace ProSeeker.Data.Models.Quiz
{
    using System;
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class Quiz : BaseDeletableModel<string>
    {
        public Quiz()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Questions = new HashSet<Question>();
        }

        public string Title { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
