using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.AInR;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class OwnerProfile:Profile,IMapperProfile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerModel>().ReverseMap();

        }
        public int Order => 0;
    }
}
