using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.StatisticalData;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class StatisticalDataMapper:Profile,IMapperProfile
    {
        public StatisticalDataMapper()
        {
            CreateMap<Production, ProductionModel>().ReverseMap();
            CreateMap<ServicesData, ServicesModel>().ReverseMap();
            CreateMap<Livestock, LivestockModel>().ReverseMap();
        }
        public int Order => 0;
    }
}
