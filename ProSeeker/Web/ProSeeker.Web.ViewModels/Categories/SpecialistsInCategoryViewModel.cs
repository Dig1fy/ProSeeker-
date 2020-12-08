namespace ProSeeker.Web.ViewModels.Categories
{
    using System;
    using System.Linq;

    using AutoMapper;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class SpecialistsInCategoryViewModel : IMapFrom<Specialist_Details>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserFullName => $"{this.UserFirstName} {this.UserLastName}";

        public string UserCityName { get; set; }

        // Accessing Specialist_Details -> JobCategory -> Name property via the AutoMapper
        public string JobCategoryName { get; set; }

        public string UserUserName { get; set; }

        public double AverageRating { get; set; }

        public int RatingsCount { get; set; }

        public string UserProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Likes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Specialist_Details, SpecialistsInCategoryViewModel>()
                .ForMember(x => x.AverageRating, opt =>
                {
                    opt.MapFrom(m => m.Ratings.Average(v => v.Value));
                })
                .ForMember(y => y.RatingsCount, opt =>
                {
                    opt.MapFrom(m => m.Ratings.Count());
                });
        }
    }
}
