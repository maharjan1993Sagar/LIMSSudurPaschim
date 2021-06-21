using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class MoAMACProfile: Profile, IMapperProfile
    {
        public MoAMACProfile()
        {
            CreateMap<MoAMACModel, MoAMAC>().ReverseMap();
        }
        public int Order => 0;
    }
}
