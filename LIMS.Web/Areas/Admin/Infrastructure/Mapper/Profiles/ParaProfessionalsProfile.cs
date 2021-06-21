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
    public class ParaProfessionalsProfile:Profile, IMapperProfile
    {
        public ParaProfessionalsProfile()
        {
            CreateMap<ParaProfessionalsModel, ParaProfessionals>().ReverseMap();
        }


       public int Order => 0;
    }
}
