namespace ProSeeker.Web.ViewModels.Ads
{
    using System;
    using System.Linq;

    using AutoMapper;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public abstract class BaseAdViewModel : IMapFrom<Ad>, IMapTo<Ad>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PreparedBudget { get; set; }

        public CitySimpleViewModel City { get; set; }

        public bool IsVip { get; set; }

        public int Views { get; set; }

        public int JobCategoryId { get; set; }

        public int UpVotesCount { get; set; }

        public int DownVotesCount { get; set; }

        public int OpinionsCount { get; set; }

        public DateTime CreatedOn { get; set; }

        public CategorySimpleViewModel JobCategory { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, AdsFullDetailsViewModel>()
               .ForMember(x => x.UpVotesCount, options =>
               {
                   options.MapFrom(p => p.Votes.Where(x => x.VoteType == VoteType.UpVote)
               .Select(x => x.VoteType).Count());
               })
               .ForMember(x => x.DownVotesCount, options =>
               {
                   options.MapFrom(p => p.Votes.Where(x => x.VoteType == VoteType.DownVote)
               .Select(x => x.VoteType).Count());
               });

            configuration.CreateMap<Ad, AdsShortDetailsViewModel>()
              .ForMember(x => x.UpVotesCount, options =>
              {
                  options.MapFrom(p => p.Votes.Where(x => x.VoteType == VoteType.UpVote)
              .Select(x => x.VoteType).Count());
              })
              .ForMember(x => x.DownVotesCount, options =>
              {
                  options.MapFrom(p => p.Votes.Where(x => x.VoteType == VoteType.DownVote)
              .Select(x => x.VoteType).Count());
              });
        }
    }
}
