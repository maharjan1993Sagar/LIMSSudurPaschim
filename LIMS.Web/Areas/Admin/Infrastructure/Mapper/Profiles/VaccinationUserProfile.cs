using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Users;
using LIMS.Web.Areas.Admin.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class VaccinationUserProfile:Profile,IMapperProfile
    {
        public VaccinationUserProfile()
        {
            CreateMap<VaccinationUser, VaccinationUserModel>().ReverseMap();

        }
        public int Order => 0;
    }
}
