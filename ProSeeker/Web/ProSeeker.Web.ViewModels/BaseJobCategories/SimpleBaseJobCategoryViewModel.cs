namespace ProSeeker.Web.ViewModels.BaseJobCategories
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class SimpleBaseJobCategoryViewModel : IMapFrom<BaseJobCategory>
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }
    }
}
