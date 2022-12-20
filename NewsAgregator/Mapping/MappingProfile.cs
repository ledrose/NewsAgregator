using AutoMapper;
using NewsAgregator.ViewModels;
using System.ServiceModel.Syndication;
using VueProjectBack.Models;

namespace VueProjectBack.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<NewsItem, NewsOutputItemModel>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate.ToString()));
        }
    }
}
