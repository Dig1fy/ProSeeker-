namespace ProSeeker.Data.Models
{
    using ProSeeker.Data.Common.Models;

    public class Service : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string SpecialistDetailsId { get; set; }

        public Specialist_Details SpecialistDetails { get; set; }
    }
}
