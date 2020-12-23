namespace ProSeeker.Web.ViewModels.Votes
{
    using System.ComponentModel.DataAnnotations;

    public class VoteInputModel
    {
        [Required]
        [MaxLength]
        public string AdId { get; set; }

        public bool IsUpVote { get; set; }
    }
}
