using AutoMapper;
using LIMS.Api.DTOs;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.DTOs.Common;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.DTOs.RationBalance;
using LIMS.Api.Extensions;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.AInR;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using LIMS.Domain.DynamicMenu;
using LIMS.Domain.GeneralCMS;
using LIMS.Domain.NewsEvent;
using LIMS.Domain.RationBalance;
using LIMS.Domain.Recording;
using LIMS.Domain.Services;
using LIMS.Domain.Vaccination;

namespace LIMS.Api.Infrastructure.Mapper.Profiles
{
    public class GeneralCMSProfile : Profile, IMapperProfile
    {
        public GeneralCMSProfile()
        {
            CreateMap<BannerDto, Banner>().ReverseMap();
            CreateMap<ContactUsDto, ContactUs>().ReverseMap();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Employee, EmployeeDto>()
                                                .ForMember(dest=>dest.Image,
                                                mo=>mo.MapFrom(src=>src.Image));
            CreateMap<GalleryDto, Gallery>();
            CreateMap<Gallery, GalleryDto>().ForMember(dest=>dest.Images,
                       mo=>mo.MapFrom(src=>src.Images));
            CreateMap<ImportantLinksDto, ImportantLinks>().ReverseMap();
            CreateMap<MainMenuDto, MainMenu>().ReverseMap();
            CreateMap<SubMenuDto, SubMenu>().ReverseMap();
            CreateMap<SubSubMenuDto, SubSubMenu>().ReverseMap();
            CreateMap<PageContentDto, PageContent>();
            CreateMap<PageContent, PageContentDto>().ForMember(dest => dest.Image,
                                                mo => mo.MapFrom(src => src.PageContentFile));
            CreateMap<NewsEventTenderDto, NewsEventTender>();
            CreateMap<NewsEventTender, NewsEventTenderDto>().ForMember(dest => dest.Image,
                                                              mo => mo.MapFrom(src => src.NewsEventFile));
            CreateMap<NewsEventFile, NewsEventFileDto>().ReverseMap();
            CreateMap<FarmLabResources, Resources>().ReverseMap();
            CreateMap<Soil, SoilDto>().ReverseMap();
            CreateMap<MarketData, MarketDto>().ReverseMap();

        }




        public int Order => 1;
    }
}
