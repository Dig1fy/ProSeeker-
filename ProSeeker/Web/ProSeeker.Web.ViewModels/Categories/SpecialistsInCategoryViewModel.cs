namespace ProSeeker.Web.ViewModels.Categories
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class SpecialistsInCategoryViewModel : IMapFrom<Specialist_Details>
    {
        public string Id { get; set; }

        public string UserFirstName { get; set; }

        public int JobCategoryId { get; set; }

        //public virtual SpecialistDetailsViewModel SpecialistDetails { get; set; }
    }
}
