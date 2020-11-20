namespace ProSeeker.Web.ViewModels.Services
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class ServiceInputModel : IMapFrom<Service>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}