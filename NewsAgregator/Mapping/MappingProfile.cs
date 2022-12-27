using AutoMapper;
using NewsAgregator.ViewModels;
using NewsAgregator.Models;

namespace NewsAgregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<NewsItem, NewsOutputItemModel>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate.ToString()))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<RegisterModel, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Roles.User));
        }
    }
}
