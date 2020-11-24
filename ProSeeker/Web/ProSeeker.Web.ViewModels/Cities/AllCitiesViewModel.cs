namespace ProSeeker.Web.ViewModels.Cities
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class AllCitiesViewModel : IMapFrom<City>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
