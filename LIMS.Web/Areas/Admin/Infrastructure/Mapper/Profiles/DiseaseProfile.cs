using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class DiseaseProfile:Profile,IMapperProfile
    {
        public DiseaseProfile()
        {
            CreateMap<Disease, DiseaseModel>().ReverseMap();
        }
        public int Order => 0;
    }
}
