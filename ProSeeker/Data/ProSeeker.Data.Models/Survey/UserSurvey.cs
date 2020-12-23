namespace ProSeeker.Data.Models.Quiz
{
    using System.ComponentModel.DataAnnotations;

    public class UserSurvey
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string SurveyId { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
