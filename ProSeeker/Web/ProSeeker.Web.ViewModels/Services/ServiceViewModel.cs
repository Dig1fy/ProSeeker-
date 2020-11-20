namespace ProSeeker.Web.ViewModels.Services
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class ServiceViewModel : IMapFrom<Service>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
