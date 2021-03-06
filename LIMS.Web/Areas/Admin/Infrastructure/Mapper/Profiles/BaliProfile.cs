using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.Bali.Aanudan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class BaliProfile: Profile, IMapperProfile
    {
        public BaliProfile()
        {
            CreateMap<BaliRegister, BaliModel>().ReverseMap();
            CreateMap<MarketData, MarketModel>().ReverseMap();
            CreateMap<Farmer, FarmerModel>().ReverseMap();
            CreateMap<IncubationCenter, IncubationCenterModel>().ReverseMap();
            CreateMap<Soil, SoilModel>().ReverseMap();
            CreateMap<Talim, TalimModel>().ReverseMap();
            CreateMap<FarmLabResources, FarmLabResourcesModel>().ReverseMap();
            CreateMap<LabambitKrishakHaru, LabambitKrishakModel>().ReverseMap();
            CreateMap<AanudanKokaryakram, AanudanModel>().ReverseMap();



        }
        public int Order { get; set; } = 0;
    }
}
