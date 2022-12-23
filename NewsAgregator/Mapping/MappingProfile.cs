using AutoMapper;
using NewsAgregator.ViewModels;
using System.ServiceModel.Syndication;
using NewsAgregator.Models;
using NewsAgregator.ViewModels;

namespace NewsAgregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<NewsItem, NewsOutputItemModel>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate.ToString()));
            CreateMap<RegisterModel, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Roles.User));
        }
    }
}
