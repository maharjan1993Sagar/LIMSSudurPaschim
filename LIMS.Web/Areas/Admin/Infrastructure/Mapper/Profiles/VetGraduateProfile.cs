using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Professionals;
using LIMS.Web.Areas.Admin.Models.Professionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class VetGraduateProfile: Profile, IMapperProfile
    {
        public VetGraduateProfile()
        {
            CreateMap<VetGraduateModel, VetGraduate>().ReverseMap();
        }


        public int Order => 0;
    }
}
