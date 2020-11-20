namespace ProSeeker.Web.ViewModels.Home
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class JobCategoriesViewModel : IMapFrom<JobCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public string ShortDescription =>
            this.Description.Length > 30 ? this.Description.Substring(0, 30) + "..." : this.Description;

        public string Url => $"/jobcategories/{this.Name.Replace(" ", "-").ToLower()}";
    }
}
