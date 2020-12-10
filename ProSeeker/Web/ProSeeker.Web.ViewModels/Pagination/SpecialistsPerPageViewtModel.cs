namespace ProSeeker.Web.ViewModels.Pagination
{
    public class SpecialistsPerPageViewtModel
    {
        public int Id { get; set; }

        public string SortBy { get; set; }

        public int CityId { get; set; }

        public int Page { get; set; }
    }
}
