namespace ProSeeker.Web.ViewModels.Categories
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CategorySimpleViewModel : IMapFrom<JobCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
