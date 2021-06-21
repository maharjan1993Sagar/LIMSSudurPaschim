using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Vaccination;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class VaccineTypeMappingProfile:Profile,IMapperProfile
    {
        public VaccineTypeMappingProfile()
        {


            CreateMap<VaccinationType, VaccinationTypeModel>().ReverseMap();
                   

           
        }

        public int Order => 0;
    }
}
