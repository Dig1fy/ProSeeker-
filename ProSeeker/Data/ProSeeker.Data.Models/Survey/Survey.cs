namespace ProSeeker.Data.Models.Quiz
{
    using System;
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class Survey : BaseDeletableModel<string>
    {
        public Survey()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Questions = new HashSet<Question>();
        }

        public string Title { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
