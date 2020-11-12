namespace ProSeeker.Web.ViewModels.Categories
{
    using System;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class SpecialistsInCategoryViewModel : IMapFrom<Specialist_Details>
    {
        public string Id { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserFullName => $"{this.UserFirstName} {this.UserLastName}";

        public string CityName { get; set; }

        // Accessing Specialist_Details -> JobCategory -> Name property via the AutoMapper
        public string JobCategoryName { get; set; }

        public string UserUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Likes { get; set; }
    }
}
