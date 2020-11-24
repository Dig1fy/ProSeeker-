namespace ProSeeker.Web.ViewModels.Raitings
{
    using System.ComponentModel.DataAnnotations;

    public class PostRatingInputModel
    {
        public string SpecialistDetailsId { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }
    }
}
