namespace ProSeeker.Web.ViewModels.Users
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    // For testing purposes only
    public class SpecialistShortDetailsViewModel : IMapFrom<Specialist_Details>
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public int JobCategoryId { get; set; }

        public bool IsVip { get; set; }
    }
}
