namespace ProSeeker.Data.Models.Quiz
{
    public class UserQuiz
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
